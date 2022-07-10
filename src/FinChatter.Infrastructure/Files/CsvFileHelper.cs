using CsvHelper;
using FinChatter.Application.Interfaces;
using System.Globalization;

namespace FinChatter.Infrastructure.Files
{
    internal class CsvFileHelper : ICsvFileHelper
    {
        private CsvReader _csvReader;
        public IEnumerable<T> GetRecords<T>(TextReader reader, CultureInfo cultureInfo = null)
        {
            _csvReader = new CsvReader(reader, cultureInfo ?? CultureInfo.InvariantCulture);
            return _csvReader.GetRecords<T>();
        }
    }
}
