using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using quotation.src.Models;
using quotation.src.Services.Refit;
using quotation.src.Services.Interfaces;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace quotation.src.Services
{
    public class QuotationService : IQuotationService
    {
        private readonly IEconomy _economyApi;
        private readonly IDatabase _redisDb;

        public QuotationService(IEconomy economyApi, IConnectionMultiplexer redis)
        {
            _economyApi = economyApi;
            _redisDb = redis.GetDatabase();
        }

        public async Task<Quotation> GetCurrencyInfo()
        {
            var cachedQuotation = await _redisDb.StringGetAsync("quotation");
            if (!cachedQuotation.IsNull)
            {
                return JsonConvert.DeserializeObject<Quotation>(cachedQuotation);
            }
            else
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


                    string json = JsonConvert.SerializeObject(quotation);

                    await _redisDb.StringSetAsync("quotation", json);

                    await _redisDb.KeyExpireAsync("quotation", TimeSpan.FromHours(24));

                    return quotation;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error getting currency information: {ex.Message}");
                }
            }
        }
    }

}
