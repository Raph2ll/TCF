using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using quotation.src.Models;
using quotation.src.Services.Refit;
using quotation.src.Services.Interfaces;

namespace quotation.src.Services
{
    public class QuotationService : IQuotationService
    {
        private readonly IEconomy _economyApi;

        public QuotationService(IEconomy economyApi)
        {
            _economyApi = economyApi;
        }

        public async Task<Quotation> GetCurrencyInfo()
        {
            try
            {
                var currencyInfo = await _economyApi.GetCurrencyInfo();

                var quotation = new Quotation();

                foreach (var pair in currencyInfo)
                {
                    switch (pair.Key)
                    {
                        case "EURBRL":
                            quotation.EUR = Convert.ToDecimal(pair.Value.High, CultureInfo.InvariantCulture);
                            break;
                        case "USDBRL":
                            quotation.USD = Convert.ToDecimal(pair.Value.High, CultureInfo.InvariantCulture);
                            break;
                        case "GBPBRL":
                            quotation.GBP = Convert.ToDecimal(pair.Value.High, CultureInfo.InvariantCulture);
                            break;
                        case "CNYBRL":
                            quotation.CNY = Convert.ToDecimal(pair.Value.High, CultureInfo.InvariantCulture);
                            break;
                    }
                }
                quotation.UpdatedAt = DateTime.Now;

                return quotation;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting currency information: {ex.Message}");
            }
        }
    }
}
