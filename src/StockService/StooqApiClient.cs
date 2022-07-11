using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService
{
    public class StooqApiClient
    {
        private readonly HttpClient _httpClient;
        public StooqApiClient(string baseUrl)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseUrl);
        }
        //TODO: manage exepctions 
        public async Task<TextReader> GetStockQuotesCsvAsync(string[] symbols)
        {
            var endPoint = GetEndpoint(symbols, EnumOutPutFormat.CSV);
            var streamResponse = await _httpClient.GetStreamAsync(endPoint);
            return new StreamReader(streamResponse);
        }

        public async Task<string> GetStockQuotesRawJsonAsync(string[] symbols)
        {
            var endPoint = GetEndpoint(symbols, EnumOutPutFormat.JSON);
            return await _httpClient.GetStringAsync(endPoint);
        }

        private static string GetEndpoint(string[] symbols, EnumOutPutFormat format)
        {
            return $"/q/l/?s={String.Join(' ', symbols)}&f=sd2t2ohlcv&h&e={format.ToString().ToLower()}";
        }
    }
}
