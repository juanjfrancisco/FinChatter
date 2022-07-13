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
        public BotReceiverService(IOptions<RabbitMqConfiguration> mqConfig, IStockApiClient stockApiClient, ICsvFileHelper csvFileHelper, IMqSender mqSender) 
            : base(mqConfig, stockApiClient, csvFileHelper, mqSender)
        {
        }

        protected override async Task MessageHandler(byte[] body)
        {
            var content = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<ChatMessage>(content);
            var symbols = GetStockCodes(message.Message);
            var quotes = await GetStockQuote(symbols);

            message.UserName = "#boot";

            if (quotes != null && quotes.Count > 0)
                message.Message = String.Join('\n', quotes.Select(quote => $"{quote.Symbol} quote is ${quote.Close} per share."));
            else
                message.Message = "No record found sorry";
            message.SentDate = DateTime.Now;

            _mqSender.SendMessage(message);
        }

        private string[] GetStockCodes(string message)
        {
            var proccesor = new Regex(@"\/stock=(?<StockCode>.*)");
            Match matches = proccesor.Match(message);

            if (matches.Success)
            {
                var stockCode = matches.Groups["StockCode"].Value.Trim();
                return stockCode.Split(',');
            }

            return Array.Empty<string>();
        }
        private async Task<List<GetStockQuotesResponse>> GetStockQuote(string [] symbols)
        {
            var quotesStream = await _stockApiClient.GetStockQuotesCsvAsync(symbols);
            var json = await _stockApiClient.GetStockQuotesRawJsonAsync(symbols);
            return _csvFileHelper.GetRecords<GetStockQuotesResponse>(quotesStream).ToList();
        }


    }
}
