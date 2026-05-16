namespace CleanArchitecture.Application.Common.Interfaces
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(string exchange, string routingKey, T message);
    }
}