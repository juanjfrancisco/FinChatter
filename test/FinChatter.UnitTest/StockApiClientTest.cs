using FinChatter.Application.Extensions;
using FinChatter.Application.Interfaces;
using FinChatter.Application.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinChatter.UnitTest
{
    [TestClass]
    public class StockApiClientTest
    {
        private readonly IStockApiClient _stooqApiClient;
        private readonly ICsvFileHelper _csvHelper;
        private readonly string[] _symbolsRequest;
        public StockApiClientTest()
        {
            _stooqApiClient = SetupSetting.StockApiClient;
            _csvHelper = SetupSetting.CsvFileHelper;
            _symbolsRequest = new string[] { "AAL.US", "AAPL.US", "ABC.US", "no", "ABC.US" };
        }

        [TestMethod]
        public async Task StooqApi_GetStockQuotesCsvTest()
        {
            var quotesStream = await _stooqApiClient.GetStockQuotesCsvAsync(_symbolsRequest);
            Assert.IsNotNull(quotesStream);

            var quotes = _csvHelper.GetRecords<GetStockQuotesResponse>(quotesStream).ToArray();
            Assert.IsNotNull(quotes);

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

            Console.WriteLine(botResponse.ToString());

            Assert.IsTrue(quotes.Length == _symbolsRequest.Length);
        }

        [TestMethod]
        public async Task StooqApi_GetStockQuotesRawJsonTest()
        {
            var quotesJson = await _stooqApiClient.GetStockQuotesRawJsonAsync(_symbolsRequest);
            Assert.IsTrue(!string.IsNullOrEmpty(quotesJson));
            Assert.IsTrue(quotesJson.IsJson());

            var quoteList = quotesJson.FromJson<QuoteList>();
            Assert.IsNotNull(quoteList);
            Assert.IsNotNull(quoteList.Quotes);
            Assert.IsTrue(quoteList.Quotes.Count == _symbolsRequest.Length);

        }
    }

    internal class QuoteList
    {
        [JsonPropertyName("symbols")]
        public List<GetStockQuotesResponse> Quotes { get; set; }
    }
}
