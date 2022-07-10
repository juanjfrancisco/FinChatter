using System.Globalization;


namespace FinChatter.Application.Interfaces
{
    public interface ICsvFileHelper
    {
        IEnumerable<T> GetRecords<T>(TextReader reader, CultureInfo cultureInfo = null);
    }
}
