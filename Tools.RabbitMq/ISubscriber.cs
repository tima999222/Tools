namespace Tools.RabbitMq
{
    public interface ISubscriber
    {
        Task ConsumeAsync(Action<string>? onConsume = null);
    }
}
