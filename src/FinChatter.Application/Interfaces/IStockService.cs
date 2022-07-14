using FinChatter.Application.Model;

namespace FinChatter.Application.Interfaces
{
    public interface IStockService
    {
        Task<GetStockQuotesResponse[]> GetStockQuote(string message);
    }
}
