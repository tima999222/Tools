using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Tools.RabbitMq.Publishers
{
    public class DefaultPublisher : IPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;
        private readonly string _exchangeName;

        public DefaultPublisher(ConnectionFactory factory, string queueName, string exchange)
        {
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _queueName = queueName;
                _exchangeName = exchange;

                _channel.ExchangeDeclare(exchange: _exchangeName,
                                    durable: true,
                                    type: ExchangeType.Fanout,
                                    autoDelete: false,
                                    arguments: null);

                _channel.QueueDeclare(queue: _queueName,
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                _channel.QueueBind(_queueName, _exchangeName, routingKey: "");
            }
            catch
            {
                throw;
            }

        }

        public async Task PublishAsync<T>(T message)
        {
            await Task.Delay(1);
            var m = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(m);

            _channel.BasicPublish(exchange: _exchangeName,
                                    routingKey: _queueName,
                                    basicProperties: null,
                                    body: body);
        }
    }
}
