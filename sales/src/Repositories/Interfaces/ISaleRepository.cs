using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sales.src.Models;

namespace sales.src.Repositories.Interfaces
{
    public interface ISaleRepository
    {
        public Task CreateSale(Sale sale, List<SaleItem> items);
        public  Task<Sale> GetSaleById(string id);
        public  Task<List<Sale>> GetAllSales();
        public Task UpdateSale(string id, Sale updatedSale);
        public Task DeleteSale(string id);
    }
}