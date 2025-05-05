
namespace Tools.RabbitMq
{
    public interface IPublisher : IDisposable
    {
        Task PublishAsync<T>(T message);
    }
}
