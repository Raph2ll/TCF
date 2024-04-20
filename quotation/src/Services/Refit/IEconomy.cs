using System;
using System.Collections.Generic;
using Refit;

namespace quotation.src.Services.Refit
{
    public interface IEconomy
    {
        [Get("/last/EUR-BRL,USD-BRL,GBP-BRL,CNY-BRL")]
        Task<Dictionary<string, CurrencyInfo>> GetCurrencyInfo();
    }

    public class CurrencyInfo
    {
        public string? Code { get; set; }
        public string? High { get; set; }
    }
}