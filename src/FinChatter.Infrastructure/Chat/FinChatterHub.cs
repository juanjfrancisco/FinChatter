using FinChatter.Application.Interfaces;
using FinChatter.Application.Model;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinChatter.Infrastructure.Chat
{
    public  class FinChatterHub : Hub
    {
        IConnectionMapping<Guid> _usersConnections;
        public FinChatterHub(IConnectionMapping<Guid> usersConnections)
        {
            _usersConnections = usersConnections;
        }
        public Task SendMessage(ChatMessage message)
        {
            message.UserName = Context.User.Identity.Name;
  
            var sendMessage = Clients.All.SendAsync("SendMessage", message);

            return sendMessage;
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
