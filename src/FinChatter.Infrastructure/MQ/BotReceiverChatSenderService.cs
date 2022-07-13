using FinChatter.Application.Interfaces;
using FinChatter.Application.Model;
using FinChatter.Infrastructure.Chat;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace FinChatter.Infrastructure.MQ
{
    internal class BotReceiverChatSenderService : BotReceiverServiceBase
    {
        private readonly IHubContext<FinChatterHub> _finChatHub;
        public BotReceiverChatSenderService(IOptions<RabbitMqConfiguration> mqConfig, IMqSender mqSender, IHubContext<FinChatterHub> finChatHub) 
            : base(mqConfig,  mqSender)
        {
            _finChatHub = finChatHub;
        }
        protected async override Task MessageHandler(byte[] body)
        {
            try
            {
                var content = Encoding.UTF8.GetString(body.ToArray());
                var clientMessage = JsonSerializer.Deserialize<ChatMessage>(content);
                await _finChatHub.Clients.All.SendAsync("SendMessage", clientMessage);
            }
            catch (Exception e)
            {
                // TO DO
            }
        }
    }
}
