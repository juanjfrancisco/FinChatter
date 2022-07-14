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
services.AddHealthChecks();

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
app.UseHealthChecks("/health");
app.MapControllers();

app.MapHub<FinChatterHub>(config.GetValue<string>("FinChatterHub"));

app.MapGet("/", context =>
{
    context.Response.Redirect("/help", permanent: false);
    return Task.FromResult(0);
});

app.Run();
