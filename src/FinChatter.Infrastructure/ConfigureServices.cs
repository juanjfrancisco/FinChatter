using FinChatter.Application.Interfaces;
using FinChatter.Infrastructure.Files;
using FinChatter.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinChatter.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Todo: maybe pass Ioptions<Configuration>
            services.AddSingleton<IStockApiClient>(new StockApiClient("https://stooq.com"));
            services.AddSingleton<ICsvFileHelper, CsvFileHelper>();
            return services;
        }
    }
}
