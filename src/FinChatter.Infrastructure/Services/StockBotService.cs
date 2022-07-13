﻿using FinChatter.Application.Interfaces;
using FinChatter.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FinChatter.Infrastructure.Services
{
    internal class StockBotService : IStockService
    {
        private readonly IStockApiClient _stockApiClient;
        private readonly ICsvFileHelper _csvFileHelper;

        public StockBotService(IStockApiClient stockApiClient, ICsvFileHelper csvFileHelper)
        {
            _stockApiClient = stockApiClient;
            _csvFileHelper = csvFileHelper;
        }

        public async Task<List<GetStockQuotesResponse>> GetStockQuote(string message)
        {
            var symbols = GetStockCodes(message);
            var quotesStream = await _stockApiClient.GetStockQuotesCsvAsync(symbols);
            return _csvFileHelper.GetRecords<GetStockQuotesResponse>(quotesStream).ToList();
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
