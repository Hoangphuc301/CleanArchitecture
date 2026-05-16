namespace CleanArchitecture.Infrastructure.Messaging
{
    public class RabbitMqSettings
    {
        public string HostName { get; set; } = null!;

        public string ExchangeName { get; set; } = null!;

        public string QueueName { get; set; } = null!;
    }
}