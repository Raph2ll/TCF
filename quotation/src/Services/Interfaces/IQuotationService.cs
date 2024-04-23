using System;
using System.Collections.Generic;
using System.Linq;
using quotation.src.Models;
using quotation.src.Services.Refit;

namespace quotation.src.Services.Interfaces
{
    public interface IQuotationService
    {
        Task<Quotation> GetCurrencyInfo();
        Task<decimal> GetCurrencyByName(string currencyName);
    }
}