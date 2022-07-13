using FinChatter.Application.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace FinChatter.Infrastructure.Extension
{
    public static class SwaggerExtension
    {

        private static string _ApiHelpTitle;
        private static string _ApiHelpDescription;
        private static string _ApiVersion = "v1";
        private static string _AssemblyVersion;

        /// <summary>
        /// Register the Swagger generator, defining 1 or more Swagger documents
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerApiHelp(this IServiceCollection services)
        {
            var assemblyObj = Assembly.GetEntryAssembly();
            var assemblyExecuting = Assembly.GetExecutingAssembly();
            var assemblyDesc = assemblyObj
                .GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
                .OfType<AssemblyDescriptionAttribute>()
                .FirstOrDefault();

            _ApiHelpTitle = assemblyObj.GetName().Name;
            _AssemblyVersion = assemblyObj.GetName().Version?.ToString();

            _ApiHelpDescription = $"{((assemblyDesc == null) ? _ApiHelpTitle : assemblyDesc.Description)} ({_AssemblyVersion})";
            _ApiHelpDescription = $"{_ApiHelpDescription}<br>{assemblyObj.GetCreationTime():yyyy.MM.dd hh:mm:ss tt}";

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(_ApiVersion,
                    new OpenApiInfo
                    {
                        Title = _ApiHelpTitle,
                        Version = _ApiVersion,
                        Description = _ApiHelpDescription,
                    });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{assemblyObj.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFile);

                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath);

                var sharedXmlFile = $"{assemblyExecuting.GetName().Name}.xml";
                var sharedXmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, sharedXmlFile);

                if (File.Exists(sharedXmlPath))
                    c.IncludeXmlComments(sharedXmlPath);

                var modelXmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FinChatter.API.Contracts.xml");

                if (File.Exists(modelXmlPath))
                    c.IncludeXmlComments(modelXmlPath);

                //Add Authorization header
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();

            });
        }

        /// <summary>
        /// Enable middleware to serve generated Swagger as a JSON endpoint.
        /// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
        /// specifying the Swagger JSON endpoint.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        public static void UseSwaggerApiHelp(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (configuration.GetSection("HelpSettings").GetValue<bool>("IsEnable"))
            {
                var docTitle = $"{_ApiHelpTitle} {_AssemblyVersion}";
                app.UseSwagger(o =>
                {
                    o.RouteTemplate = "help/{documentName}/help.json";
                });

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/help.json", docTitle);
                    c.DocumentTitle = docTitle;
                    c.RoutePrefix = "help";
                });
            }
        }
    }
}
