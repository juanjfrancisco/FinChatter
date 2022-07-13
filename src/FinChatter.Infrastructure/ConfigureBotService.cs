using FinChatter.Application.Interfaces;
using FinChatter.Application.Model;
using FinChatter.Infrastructure.Files;
using FinChatter.Infrastructure.MQ;
using FinChatter.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace FinChatter.Infrastructure
{
    public static class ConfigureBotService
    {

        public static IServiceCollection AddInfrastructureBotServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMQ"));
            services.Configure<StooqAPIConfiguration>(configuration.GetSection("StooqAPI"));
            services.AddSingleton<IStockApiClient, StockApiClient>();
            services.AddSingleton<ICsvFileHelper, CsvFileHelper>();
            services.AddSingleton<IMqSender, BotSender>();
            services.AddHostedService<BotReceiverService>();
            AddHealthCheck(services, configuration);
           
            return services;
        }

        private static void AddHealthCheck(IServiceCollection services, IConfiguration configuration)
        {
            #region Rabbitmq helthcheck
            var rabbitConfig = configuration.GetSection("RabbitMQ");
            var mqUser = rabbitConfig.GetValue<string>("UserName");
            var mqPassword = rabbitConfig.GetValue<string>("Password");
            var mqHost = rabbitConfig.GetValue<string>("HostName");

            var factory = new ConnectionFactory()
            {
                Uri = new Uri($"amqp://{mqUser}:{mqPassword}@{mqHost}:5672"),
                AutomaticRecoveryEnabled = true
            };

            services.AddHealthChecksUI(opt =>
            {
                opt.SetEvaluationTimeInSeconds(5); 
                opt.MaximumHistoryEntriesPerEndpoint(60); 
                opt.SetApiMaxActiveRequests(1); 

                opt.AddHealthCheckEndpoint("RabbitMQ Health", "/health"); 
            }).AddInMemoryStorage();


            try
            {
                var connection = factory.CreateConnection();
                services
                     .AddSingleton(connection)
                     .AddHealthChecks()
                     .AddRabbitMQ();
            }
            catch (Exception e)
            {
                //
            }
            #endregion
        }
    }
}
