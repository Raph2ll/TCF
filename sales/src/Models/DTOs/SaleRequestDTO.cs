using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sales.src.Models.DTOs
{
    public class SaleRequestDTO
    {
        public string? ClientId { get; set; }

        public List<SaleItemRequestDTO>? Items { get; set; }

    }
}
