using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FinChatter.UnitTest
{
    [TestClass]
    public class MiscTest
    {

        [TestMethod]
        public void RegularExGetStockCodes()
        {
            var firstElement = "app.pl";
            var secondElement = "pp.l";
            var msg = GetStockCodes($"/stock={firstElement},{secondElement}");
            Assert.IsNotNull(msg);
            Assert.IsTrue(msg.Length == 2);
            Assert.IsTrue(msg[0] == firstElement && msg[1] == secondElement);
        }
        private string[] GetStockCodes(string message)
        {
            var proccesor = new Regex(@"\/stock=(?<StockCode>.*)");
            Match matches = proccesor.Match(message);

            if (matches.Success)
            {
                var stockCode = matches.Groups["StockCode"].Value.Trim();
                return stockCode.Split(',');
            }

            return Array.Empty<string>();
        }
    }
}
