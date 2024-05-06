using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using sales.src.Models;

namespace sales.src.Services.Interfaces
{
    public interface ISaleService
    {
        Task CreateSale(Sale sale);
        Task<Sale> GetSaleById(string id);
        Task<List<Sale>> GetAllSales();
        Task UpdateSale(string id, Sale updatedSale);
        Task DeleteSale(string id);
    }
}
