using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Tools.RabbitMq.Subscribers
{
    public class DefaultSubscriber : ISubscriber
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;

        public DefaultSubscriber(ConnectionFactory factory, string queueName)
        {
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _queueName = queueName;

                _channel.QueueDeclare(queue: _queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
            }
            catch
            {
                throw;
            }
            
        }

        public async Task ConsumeAsync(Action<string>? onConsume = null)
        {
            await Task.Delay(1);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    onConsume?.Invoke(message);
                }
                catch
                {
                    throw;
                }
            };

            try
            {
                _channel.BasicConsume(
                    queue: _queueName,
                    autoAck: true,
                    consumer: consumer
                );
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                _channel?.Close();
                _channel?.Dispose();
                _connection?.Close();
                _connection?.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error disposing RabbitMQ subscriber: {ex.Message}");
            }
        }
    }
}
