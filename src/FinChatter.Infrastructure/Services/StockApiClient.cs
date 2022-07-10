using FinChatter.Application.Interfaces;
using StockService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinChatter.Infrastructure.Services
{
    internal class StockApiClient : StooqApiClient, IStockApiClient
    {
        public StockApiClient(string baseUrl) : base(baseUrl)
        {

        }
    }
}
