using FinChatter.Application.Model;

namespace FinChatter.Application.Interfaces
{
    public interface IStockService
    {
        Task<List<GetStockQuotesResponse>> GetStockQuote(string message);
    }
}
