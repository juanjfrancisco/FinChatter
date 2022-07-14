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
            var hubUrl = await _localStorage.GetItemAsync<string>("finChatterUrl");

            ChatHubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl, options =>
                {
                    options.AccessTokenProvider = async () =>
                    {
                        return await _localStorage.GetItemAsync<string>("authToken");
                    };
                })
                .Build();

            ChatHubConnection.On<ChatMessage>("SendMessage", handler);

            await ChatHubConnection.StartAsync();
            await SendAsync($"[Notice] {username} joined chat room.", username);
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
