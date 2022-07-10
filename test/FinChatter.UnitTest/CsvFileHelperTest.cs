using FinChatter.Application.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using FinChatter.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using FinChatter.Application.Model;

namespace FinChatter.UnitTest
{
    [TestClass]
    public class CsvFileHelperTest
    {
      
        private readonly ICsvFileHelper _helper;
        public CsvFileHelperTest()
        {
            _helper = SetupSetting.CsvFileHelper;
        }
        [TestMethod]
        public void GetRecordsFromCsvFile()
        {
            using (var reader = new StreamReader("Resources\\CsvFiles\\quote.csv"))
            {
                var records = _helper.GetRecords<GetStockQuotesResponse>(reader).ToList();
                Assert.IsNotNull(records);
                Assert.IsTrue(records.Count > 0);
            }
        }
    }

}