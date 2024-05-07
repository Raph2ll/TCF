using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sales.src.Models.DTOs
{
    public class SaleItemRequestDTO
    {
        public string? SellId { get; set; }

        public string? ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
