namespace Tools.RabbitMq
{
    public interface ISubscriber : IDisposable
    {
        Task ConsumeAsync(Action<string>? onConsume = null);
    }
}
