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

        public async Task StartConnection(string username, string groupName, Action<ChatMessage> handler, 
            Action<IList<ChatMessage>> handlerCachedMessages, Action<IList<ChatRoom>> handlerChatRooms,
            Action<string> handlerUpdateGroup)
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
            ChatHubConnection.On<IList<ChatRoom>>("GetChatRooms", handlerChatRooms);
            ChatHubConnection.On<IList<ChatMessage>>("CachedMessages", handlerCachedMessages);
            ChatHubConnection.On<string>("UpdateNewGroup", handlerUpdateGroup);
            
            await ChatHubConnection.StartAsync();
            await SendAsync($"[Notice] {username} joined chat room.", username, groupName);
        }

        public async Task SendAsync(string message, string username, string groupName)
        {
            if (!string.IsNullOrWhiteSpace(message))
                await ChatHubConnection.SendAsync("SendMessage", new ChatMessage { UserName = username, Message = message, GroupName = groupName });
        }

        public void SaveMessageToCache(ChatMessage message)
        {
            if (message == null && !string.IsNullOrEmpty(message.Message))
                ChatHubConnection.SendAsync("CacheMessages", message).Wait();
        }

        public async Task AddNewGroup(string groupName)
        {
            if (!string.IsNullOrEmpty(groupName))
                await ChatHubConnection.SendAsync("AddNewGroup", groupName);
        }

        public async Task DisconnectAsync(string username, string groupName)
        {
            await SendAsync($"[Notice] {username} left chat room.", username, groupName);

            await ChatHubConnection.StopAsync();
            await ChatHubConnection.DisposeAsync();

            ChatHubConnection = null;
        }
    }
}
