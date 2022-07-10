using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinChatter.Application.Interfaces
{
    public interface IStockApiClient
    {
        Task<TextReader> GetStockQuotesCsvAsync(string[] symbols);
        Task<string> GetStockQuotesRawJsonAsync(string[] symbols);
    }
}
