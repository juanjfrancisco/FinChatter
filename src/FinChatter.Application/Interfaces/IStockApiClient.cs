
namespace FinChatter.Application.Interfaces
{
    public interface IStockApiClient
    {
        Task<TextReader> GetStockQuotesCsvAsync(string[] symbols);
        Task<string> GetStockQuotesRawJsonAsync(string[] symbols);
    }
}
