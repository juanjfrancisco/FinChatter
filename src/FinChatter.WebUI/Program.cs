using FinChatter.API.Client;
using FinChatter.WebUI.Core;
using FinChatter.WebUI.Core.AuthProviders;
using FinChatter.WebUI.Core.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, TestAuthStateProvider>();
builder.Services.AddSingleton<IFinChatterApiClient>(RestService.For<IFinChatterApiClient>("https://localhost:7208"));
builder.Services.AddSingleton<FinChatterApiService>();

await builder.Build().RunAsync();
