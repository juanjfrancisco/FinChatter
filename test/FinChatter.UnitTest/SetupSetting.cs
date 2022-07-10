using FinChatter.Application.Interfaces;
using FinChatter.Infrastructure;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinChatter.UnitTest
{
    internal static class SetupSetting
    {
        public static ICsvFileHelper CsvFileHelper { get; private set; }
        public static IStockApiClient? StockApiClient { get; internal set; }

        static SetupSetting()
        {
            IConfiguration appSettings = null;
            //Startup startup = null;
            IServiceCollection serviceCollection = null;

            var service = WebHost
                .CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.Sources.Clear();
                    config.AddConfiguration(hostingContext.Configuration);
                    config.AddJsonFile("appsettings.json");
                    appSettings = config.Build();


                }).ConfigureServices(sc =>
                {
                    sc.AddInfrastructureServices(appSettings);
                    //startup.ConfigureServices(sc);
                    serviceCollection = sc;
                })
            .UseStartup<EmptyStartup>()
            .Build();

            CsvFileHelper = service.Services.GetService(typeof(ICsvFileHelper)) as ICsvFileHelper;
            StockApiClient = service.Services.GetService(typeof(IStockApiClient)) as IStockApiClient;
        }
       
    }

    public class EmptyStartup
    {
        public EmptyStartup(IConfiguration _) { }

        public void ConfigureServices(IServiceCollection _) { }

        public void Configure(IApplicationBuilder _) { }
    }
}
