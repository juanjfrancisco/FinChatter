using FinChatter.Application.Interfaces;
using FinChatter.Application.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FinChatter.Infrastructure.MQ
{
    internal abstract class BotReceiverServiceBase : BackgroundService
    {
        protected readonly IOptions<RabbitMqConfiguration> _mqConfiguration;
        protected readonly IStockApiClient _stockApiClient;
        protected readonly ICsvFileHelper _csvFileHelper;
        protected readonly IMqSender _mqSender;
        protected IConnection _mqConnection;
        protected IModel _mqChannel;

        public BotReceiverServiceBase(IOptions<RabbitMqConfiguration> mqConfig, IStockApiClient stockApiClient, ICsvFileHelper csvFileHelper, IMqSender mqSender)
        {
            _mqConfiguration = mqConfig;
            _stockApiClient = stockApiClient;
            _csvFileHelper = csvFileHelper;
            _mqSender = mqSender;
            StartRabbitMQ();
        }

        private void StartRabbitMQ()
        {
            var connFactory = new ConnectionFactory
            {
                HostName = _mqConfiguration.Value.HostName,
                UserName = _mqConfiguration.Value.UserName,
                Password = _mqConfiguration.Value.Password,
            };

            _mqConnection = connFactory.CreateConnection();
            _mqConnection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _mqChannel = _mqConnection.CreateModel();
            _mqChannel.QueueDeclare(queue: _mqConfiguration.Value.QueueNameToListen, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_mqChannel);

            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                MessageHandler(body).Wait();

                _mqChannel.BasicAck(ea.DeliveryTag, false);
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerCancelled;

            _mqChannel.BasicConsume(_mqConfiguration.Value.QueueNameToListen, false, consumer);

            return Task.CompletedTask;
        }

        protected abstract Task MessageHandler(byte[] body);


        #region Events
        protected void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }
        protected void OnConsumerCancelled(object sender, ConsumerEventArgs e) { }
        protected void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
        protected void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
        protected void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
        #endregion
    }
}
