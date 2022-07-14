using Blazored.LocalStorage;
using FinChatter.WebUI.Core.Model;
using Microsoft.AspNetCore.SignalR.Client;

namespace FinChatter.WebUI.Core.Service
{
    public class ChatClient
    {
        //private HubConnection _hubConnection;
        private readonly ILocalStorageService _localStorage;

        public HubConnection ChatHubConnection { get; set; }
        public ChatClient(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task StartConnection(string username, Action<ChatMessage> handler)
        {
            Console.WriteLine(username);
            var hubUrl = await _localStorage.GetItemAsync<string>("finChatterUrl");
            var token = await _localStorage.GetItemAsync<string>("authToken");
            Console.WriteLine($"Get url: {hubUrl} token: {token}");
            ChatHubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl, options =>
                {
                    options.AccessTokenProvider = async () =>
                    {
                        return token;
                    };
                })
                .Build();

            Console.WriteLine("after build");

            ChatHubConnection.On<ChatMessage>("SendMessage", handler);

            Console.WriteLine("after connection");
            await ChatHubConnection.StartAsync();
            Console.WriteLine("after start");

            await SendAsync($"[Notice] {username} joined chat room.", username);

            Console.WriteLine("after notice");
        }

        public async Task SendAsync(string message, string username)
        {
            if (!string.IsNullOrWhiteSpace(message))
                await ChatHubConnection.SendAsync("SendMessage", new ChatMessage { UserName = username, Message = message });
        }

        public async Task DisconnectAsync(string username)
        {
            await SendAsync($"[Notice] {username} left chat room.", username);

            await ChatHubConnection.StopAsync();
            await ChatHubConnection.DisposeAsync();

            ChatHubConnection = null;
        }
    }
}
