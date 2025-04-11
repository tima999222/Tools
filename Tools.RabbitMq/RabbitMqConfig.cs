using RabbitMQ.Client;
using System.Reflection;
using System.Text;
using Tools.Configurations;
using Tools.Environment;

namespace Tools.RabbitMq
{
    public class RabbitMqConfig : ConfigurationBase
    {
        private ConnectionFactory? _connectionFactory;

        [EnvironmentVariableKey("RABBITMQ_HOST")]
        public string Host { get; set; }

        [EnvironmentVariableKey("RABBITMQ_PORT")]
        public int Port { get; set; }

        [EnvironmentVariableKey("RABBITMQ_VIRTUAL_HOST")]
        public string VirtualHost { get; set; }

        [EnvironmentVariableKey("RABBITMQ_USERNAME")]
        public string Username { get; set; }

        [EnvironmentVariableKey("RABBITMQ_PASSWORD", false)]
        public string Password { get; set; }

        [EnvironmentVariableKey("RABBITMQ_QUEUE_NAME", defaultValue: "")]
        public string QueueName { get; set; }

        [EnvironmentVariableKey("RABBITMQ_EXCHANGE_NAME", defaultValue: "")]

        public ConnectionFactory ConnectionFactory
        {
            get
            {
                if (_connectionFactory == null)
                {
                    _connectionFactory = new ConnectionFactory
                    {
                        HostName = Host,
                        Port = Port,
                        VirtualHost = VirtualHost,
                        UserName = Username,
                        Password = Password,
                        ClientProvidedName = Assembly.GetExecutingAssembly().FullName
                    };
                }
                return _connectionFactory;
            }
        }
    }
}
