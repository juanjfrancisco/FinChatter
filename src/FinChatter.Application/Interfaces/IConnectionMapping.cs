
namespace FinChatter.Application.Interfaces
{
    public interface IConnectionMapping<TKey>
    {
        int Count { get; }
        void Add(TKey key, string connectionId);
        IEnumerable<string> GetConnections(TKey key);
        void Remove(TKey key, string connectionId);
    }
}
