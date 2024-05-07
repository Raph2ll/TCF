using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace quotation.src.Models
{
    public class Quotation
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