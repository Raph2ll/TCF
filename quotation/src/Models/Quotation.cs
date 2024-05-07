using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace quotation.src.Models
{
    public class Quotation
    {
        public decimal EUR { get; set; }
        public decimal USD { get; set; }
        public decimal GBP { get; set; }
        public decimal CNY { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}