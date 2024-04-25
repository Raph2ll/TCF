using System;
using System.Collections.Generic;
using Refit;
using System.Text.Json.Serialization;

namespace product.Services.Refit
{
    public interface IQuotation
    {
        [Get("/api")]
        Task<CurrencyInfo> GetCurrencyInfo();
    }
    public class CurrencyInfo
    {
        [JsonPropertyName("EUR")]
        public decimal EUR { get; set; }
        [JsonPropertyName("USD")]
        public decimal USD { get; set; }
        [JsonPropertyName("GBP")]
        public decimal GBP { get; set; }
        [JsonPropertyName("CNY")]
        public decimal CNY { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}