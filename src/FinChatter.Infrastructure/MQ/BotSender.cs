using FinChatter.Application.Interfaces;
using FinChatter.Application.Model;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace FinChatter.Infrastructure.MQ
{
    internal class BotSender : IMqSender
    {
        private readonly IOptions<RabbitMqConfiguration> _mqConfiguration;
        private IConnection _mqConnection;

        public BotSender(IOptions<RabbitMqConfiguration> mqConfiguration)
        {
            _mqConfiguration = mqConfiguration;
            CreateConnection();
        }

        private void CreateConnection()
        {
            var connFactory = new ConnectionFactory
            {
                HostName = _mqConfiguration.Value.HostName,
                UserName = _mqConfiguration.Value.UserName,
                Password = _mqConfiguration.Value.Password,
            };

            _mqConnection = connFactory.CreateConnection();
        }

        private bool ConnectionExists()
        {
            if (_mqConnection != null)
                return true;

            CreateConnection();

            return _mqConnection != null;
        }

        public void SendMessage(ChatMessage message)
        {
            if (ConnectionExists())
            {
                using var channel = _mqConnection.CreateModel();
                channel.QueueDeclare(queue: _mqConfiguration.Value.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: _mqConfiguration.Value.QueueName, basicProperties: null, body: body);
            }
        }
    }
}
