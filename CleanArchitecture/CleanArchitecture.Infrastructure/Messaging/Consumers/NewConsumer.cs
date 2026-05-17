using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using CleanArchitecture.Application.Features.Menu.Events;
using System.Text;
using System.Text.Json;
using CleanArchitecture.Infrastructure.Persistence.Mongo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CleanArchitecture.Application.Features.New.Events;
using MongoDB.Driver;

public class NewEventConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NewEventConsumer> _logger;
    private readonly RabbitMqNewSettings _settings;

    public NewEventConsumer(IServiceProvider serviceProvider, ILogger<NewEventConsumer> logger, IOptions<RabbitMqNewSettings> options)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _settings = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("NewEventConsumer đang khởi tạo...");

        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Đang thử kết nối tới RabbitMQ tại {Host}...", _settings.HostName);

                using var connection = await factory.CreateConnectionAsync(stoppingToken);
                using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

                // Khai báo Exchange và Queue bền vững (Durable = true)
                await channel.ExchangeDeclareAsync(_settings.ExchangeName, ExchangeType.Direct, durable: true, cancellationToken: stoppingToken);
                await channel.QueueDeclareAsync(_settings.QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);

                // Bind các routing key tương ứng
                await channel.QueueBindAsync(_settings.QueueName, _settings.ExchangeName, "new.created", cancellationToken: stoppingToken);
                await channel.QueueBindAsync(_settings.QueueName, _settings.ExchangeName, "new.updated", cancellationToken: stoppingToken);
                await channel.QueueBindAsync(_settings.QueueName, _settings.ExchangeName, "new.deleted", cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(channel);

                consumer.ReceivedAsync += async (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;

                        using var scope = _serviceProvider.CreateScope();
                        var mongoDb = scope.ServiceProvider.GetRequiredService<MongoDbContext>();

                        _logger.LogInformation("Nhận được event với {RoutingKey}", routingKey);

                        switch (routingKey)
                        {
                            case "new.created":
                                var createdData = JsonSerializer.Deserialize<NewCreatedEvent>(message);
                                if (createdData != null)
                                {
                                    await mongoDb.NewLogs.InsertOneAsync(new NewLog
                                    {
                                        NewId = createdData.NewsId,
                                        Title = createdData.Title,
                                        Slug = createdData.Slug,
                                        Content = createdData.Content,
                                        CreatedAt = DateTime.UtcNow
                                    });
                                    _logger.LogInformation("Đã thêm thành công {Id} vào MongoDB", createdData.NewsId);
                                }
                                break;

                            case "new.updated":
                                var updatedData = JsonSerializer.Deserialize<NewUpdatedEvent>(message);
                                if (updatedData != null)
                                {
                                    var filter = Builders<NewLog>.Filter.Eq(x => x.NewId, updatedData.NewId);
                                    var update = Builders<NewLog>.Update
                                        .Set(x => x.Title, updatedData.NewTitle)
                                        .Set(x => x.Slug, updatedData.Slug)
                                        .Set(x => x.Content, updatedData.Content);

                                    await mongoDb.NewLogs.UpdateOneAsync(filter, update);
                                    _logger.LogInformation("Đã sửa thành công {Id} trong MongoDB", updatedData.NewId);
                                }
                                break;

                            case "new.deleted":
                                var deletedData = JsonSerializer.Deserialize<NewDeletedEvent>(message);
                                if (deletedData != null)
                                {
                                    var filter = Builders<NewLog>.Filter.Eq(x => x.NewId, deletedData.NewId);
                                    await mongoDb.NewLogs.DeleteOneAsync(filter);
                                    _logger.LogInformation("Đã xóa thành công {Id} khỏi MongoDB", deletedData.NewId);
                                }
                                break;
                        }

                        await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Lỗi khi xử lý tin nhắn từ RabbitMQ");
                        await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
                    }
                };

                await channel.BasicConsumeAsync(_settings.QueueName, autoAck: false, consumer, stoppingToken);

                _logger.LogInformation("MenuEventConsumer đã kết nối và đang lắng nghe ngầm");

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Không thể kết nối tới RabbitMQ Broker {Message}", ex.Message);

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}