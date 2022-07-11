using FinChatter.Application.Interfaces;
using FinChatter.Application.Model;
using FinChatter.Infrastructure.Chat;
using FinChatter.Infrastructure.Files;
using FinChatter.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FinChatter.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TokenManagerConfiguration>(configuration.GetSection("JwtConfig"));
            services.AddSingleton<ITokenManager, TokenManager>();
            services.AddSingleton<IStockApiClient>(new StockApiClient("https://stooq.com"));
            services.AddSingleton<ICsvFileHelper, CsvFileHelper>();
            AddAuthentication(services);
            services.AddSingleton(typeof(IConnectionMapping<>), typeof(ConnectionMapping<>));
            services.AddSignalR();
            return services;
        }


        private static void AddAuthentication(IServiceCollection services)
        {
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                ITokenManager tokenManager = serviceProvider.GetRequiredService<ITokenManager>();
                services.AddAuthentication(config =>
                {
                    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
              .AddJwtBearer(options =>
              {
                  options.RequireHttpsMetadata = false;
                  options.SaveToken = true;
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenManager.IssuerSigningKey)),
                      ValidateIssuer = false,
                      ValidateAudience = false,
                      ClockSkew = TimeSpan.Zero,
                      RequireExpirationTime = false,

                  };
              });
            }
        }
    }
}
