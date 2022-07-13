using FinChatter.Application.Interfaces;
using FinChatter.Application.Model;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace FinChatter.Infrastructure.MQ
{
    internal class BotReceiverService : BotReceiverServiceBase
    {
        private readonly IStockService _stockService;
        public BotReceiverService(IOptions<RabbitMqConfiguration> mqConfig, IMqSender mqSender, IStockService stockService)
            : base(mqConfig, mqSender)
        {
            _stockService = stockService;
        }

        protected override async Task MessageHandler(byte[] body)
        {
            var content = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<ChatMessage>(content);
            var quotes = await _stockService.GetStockQuote(message.Message);

            message.UserName = "#boot";

            if (quotes != null && quotes.Count > 0)
                message.Message = String.Join('\n', quotes.Select(quote => $"{quote.Symbol} quote is ${quote.Close} per share."));
            else
                message.Message = "No record found sorry";
            message.SentDate = DateTime.Now;

            _mqSender.SendMessage(message);
        }

    }
}
