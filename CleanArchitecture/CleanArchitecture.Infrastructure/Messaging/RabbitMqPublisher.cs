using CleanArchitecture.Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using CleanArchitecture.Infrastructure.Messaging;

namespace CleanArchitecture.Infrastructure.Services
{
    public class RabbitMqPublisher : IMessagePublisher
    {
        private readonly ConnectionFactory _factory;
        private readonly RabbitMqSettings _settings;

        public RabbitMqPublisher(IOptions<RabbitMqSettings> options)
        {
            _settings = options.Value;

            _factory = new ConnectionFactory()
            {
                HostName = _settings.HostName
            };
        }

        public async Task PublishAsync<T>(string exchange, string routingKey, T message)
        {
            using var connection = await _factory.CreateConnectionAsync();

            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: exchange, type: ExchangeType.Direct, durable: true);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            await channel.BasicPublishAsync(exchange: exchange, routingKey: routingKey, body: body);
        }
    }
}