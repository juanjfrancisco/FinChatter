using FinChatter.Infrastructure;
using FinChatter.Infrastructure.Chat;
using FinChatter.Infrastructure.Extension;

var builder = WebApplication.CreateBuilder(args);
var services  = builder.Services;
var config = builder.Configuration;

services.AddControllers();
services.AddEndpointsApiExplorer();

services.AddSwaggerApiHelp();

services.AddInfrastructureServices(config);

var app = builder.Build();

app.UseAuthentication();

app.UseSwaggerApiHelp(config);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<FinChatterHub>("/hubs/finchatter");

app.Run();
