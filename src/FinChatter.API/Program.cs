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


builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("https://localhost:7045").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();



app.UseAuthentication();

app.UseSwaggerApiHelp(config);

app.UseCors("corsapp");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<FinChatterHub>(config.GetValue<string>("FinChatterHub"));

app.Run();
