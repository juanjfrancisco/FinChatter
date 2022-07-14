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
            message.UserName = "#boot";
            message.SentDate = DateTime.Now;
            GetStockQuotesResponse[] quotes;
            try
            {
                quotes = await _stockService.GetStockQuote(message.Message);
            }
            catch (Exception ex)
            {
                message.Message = "Sorry the stock service is not avaliable. Try again later.";
                _mqSender.SendMessage(message);
                //Todo: implement log.
                Console.WriteLine(ex.Message);
                return;
            }

            if (quotes != null && quotes.Length > 0)
            {
                StringBuilder botResponse = new StringBuilder(quotes.Length);
                int len = quotes.Length; 
                if (quotes.Length > 5)
                {
                    len = 5;
                    botResponse.AppendLine("We only allow 5 simultaneous queries. Here are the first 5 stock code:");
                }

                for (int i = 0; i < len; i++)
                {
                    if (string.IsNullOrEmpty(quotes[i].Close) || quotes[i].Close.ToUpper().Equals("N/D"))
                        botResponse.AppendLine($"{quotes[i].Symbol} quote was not found. Check if the stock code is correct.");
                    else
                        botResponse.AppendLine($"{quotes[i].Symbol} quote is ${quotes[i].Close} per share.");

                }

                message.Message = botResponse.ToString();
            }
            else
                message.Message = "No record found sorry";
            message.SentDate = DateTime.Now;

            _mqSender.SendMessage(message);
        }

    }
}
