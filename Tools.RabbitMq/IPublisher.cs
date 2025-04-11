
namespace Tools.RabbitMq
{
    public interface IPublisher
    {
        Task PublishAsync<T>(T message);
    }
}
