using FinChatter.Application.Interfaces;
using FinChatter.Application.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FinChatter.Infrastructure.Chat
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FinChatterHub : Hub
    {
        private readonly IConnectionMapping<Guid> _usersConnections;
        private readonly IMqSender _mqSender;
        private static IList<ChatMessage> _messages = new List<ChatMessage>();
        private static IList<ChatRoom> _chatRooms = new List<ChatRoom> { new ChatRoom { GroupName = "Public" } };
        public FinChatterHub(IConnectionMapping<Guid> usersConnections, IMqSender mqSender)
        {
            _usersConnections = usersConnections;
            _mqSender = mqSender;
        }

        public async Task SendMessage(ChatMessage message)
        {
            if (message != null)
            {
                if (string.IsNullOrEmpty(message.GroupName))
                    message.GroupName = ChatMessage.DefaultGroupName;

                await CacheMessages(message);

                if (IsCommand(message))
                    _mqSender.SendMessage(message);

                await Clients.All.SendAsync("SendMessage", message);
            }
        }

        public async Task CacheMessages(ChatMessage message)
        {
            if (string.IsNullOrEmpty(message.Message) || message.Message.Contains("[Notice]"))
                return;

            _messages.Add(message);
            if (_messages.Count() > 50)
                _messages.RemoveAt(0);
        }

        public async Task AddNewGroup(string groupName)
        {
            if (_chatRooms.Any(a => a.GroupName == groupName))
                return;

            _chatRooms.Add(new ChatRoom { GroupName = groupName });
            await Clients.All.SendAsync("UpdateNewGroup", groupName);
        }

        private static bool IsCommand(ChatMessage message)
        {
            //Todo: improve command options
            return message != null && !string.IsNullOrEmpty(message.Message) && message.Message.ToLower().Contains("/stock=");
        }

        public override Task OnConnectedAsync()
        {
            _usersConnections.Add(GetUserId(), Context.ConnectionId);
            SendUsersGroups();
            Clients.AllExcept(Context.ConnectionId).SendAsync("NewUserConnected", new ChatMessage
            {
                UserName = "Service",
                Message = "New user connected",
                SentDate = DateTime.Now
            });

            Clients.Caller.SendAsync("CachedMessages", _messages);
            Clients.Caller.SendAsync("GetChatRooms", _chatRooms);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = GetUserId();
            _usersConnections.Remove(userId, Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        #region Private method

        /// <summary>
        /// Send all users groups on connect
        /// </summary>
        private void SendUsersGroups()
        {
            //Send message to all groups 
        }

        private Guid GetUserId()
        {
            //Todo: get userid or username
            return Guid.NewGuid();
        }

        #endregion
    }
}
