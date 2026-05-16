using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using CleanArchitecture.Application.Features.Menu.Events;
using System.Text;
using System.Text.Json;
using CleanArchitecture.Infrastructure.Persistence.Mongo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using CleanArchitecture.Infrastructure.Messaging;
using Microsoft.Extensions.Options;

public class MenuEventConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MenuEventConsumer> _logger;
    private readonly RabbitMqSettings _settings;

    public MenuEventConsumer(IServiceProvider serviceProvider, ILogger<MenuEventConsumer> logger, IOptions<RabbitMqSettings> options)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _settings = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("MenuEventConsumer đang khởi tạo...");

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
                await channel.QueueBindAsync(_settings.QueueName, _settings.ExchangeName, "menu.created", cancellationToken: stoppingToken);
                await channel.QueueBindAsync(_settings.QueueName, _settings.ExchangeName, "menu.updated", cancellationToken: stoppingToken);
                await channel.QueueBindAsync(_settings.QueueName, _settings.ExchangeName, "menu.deleted", cancellationToken: stoppingToken);

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
                            case "menu.created":
                                var createdData = JsonSerializer.Deserialize<MenuCreatedEvent>(message);
                                if (createdData != null)
                                {
                                    await mongoDb.MenuLogs.InsertOneAsync(new MenuLog
                                    {
                                        MenuId = createdData.MenuId,
                                        MenuName = createdData.MenuName,
                                        Slug = createdData.Slug,
                                        DisplayOrder = createdData.DisplayOrder,
                                        CreatedAt = DateTime.UtcNow
                                    });
                                    _logger.LogInformation("Đã thêm thành công {Id} vào MongoDB", createdData.MenuId);
                                }
                                break;

                            case "menu.updated":
                                var updatedData = JsonSerializer.Deserialize<MenuUpdatedEvent>(message);
                                if (updatedData != null)
                                {
                                    var filter = Builders<MenuLog>.Filter.Eq(x => x.MenuId, updatedData.MenuId);
                                    var update = Builders<MenuLog>.Update
                                        .Set(x => x.MenuName, updatedData.NewMenuName)
                                        .Set(x => x.Slug, updatedData.Slug)
                                        .Set(x => x.DisplayOrder, updatedData.DisplayOrder);

                                    await mongoDb.MenuLogs.UpdateOneAsync(filter, update);
                                    _logger.LogInformation("Đã sửa thành công {Id} trong MongoDB", updatedData.MenuId);
                                }
                                break;

                            case "menu.deleted":
                                var deletedData = JsonSerializer.Deserialize<MenuDeletedEvent>(message);
                                if (deletedData != null)
                                {
                                    var filter = Builders<MenuLog>.Filter.Eq(x => x.MenuId, deletedData.MenuId);
                                    await mongoDb.MenuLogs.DeleteOneAsync(filter);
                                    _logger.LogInformation("Đã xóa thành công {Id} khỏi MongoDB", deletedData.MenuId);
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