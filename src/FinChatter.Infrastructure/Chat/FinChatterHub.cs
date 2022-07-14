using FinChatter.Application.Interfaces;
using FinChatter.Application.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FinChatter.Infrastructure.Chat
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public  class FinChatterHub : Hub
    {
        private readonly IConnectionMapping<Guid> _usersConnections;
        private readonly IMqSender _mqSender;
        public FinChatterHub(IConnectionMapping<Guid> usersConnections, IMqSender mqSender)
        {
            _usersConnections = usersConnections;
            _mqSender = mqSender;
        }
        public async Task SendMessage (ChatMessage message)
        {
            if (IsCommand(message))
                _mqSender.SendMessage(message);

                await Clients.All.SendAsync("SendMessage", message);
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
