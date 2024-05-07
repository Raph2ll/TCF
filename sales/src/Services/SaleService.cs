using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using sales.src.Models;
using sales.src.Repositories.Interfaces;
using sales.src.Services.Interfaces;

namespace sales.src.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;

        public SaleService(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task CreateSale(Sale sale)
        {
            sale.CreatedAt = DateTime.UtcNow;
            sale.UpdatedAt = DateTime.UtcNow;

            foreach (var item in sale.Items)
            {
                await _saleItemsCollection.InsertOneAsync(item);
            }

            await _salesCollection.InsertOneAsync(sale);
        }


        public async Task<Sale> GetSaleById(string id)
        {
            return await _saleRepository.GetSaleById(id);
        }

        public async Task<List<Sale>> GetAllSales()
        {
            return await _saleRepository.GetAllSales();
        }

        public async Task UpdateSale(string id, Sale updatedSale)
        {
            updatedSale.UpdatedAt = DateTime.UtcNow;
            await _saleRepository.UpdateSale(id, updatedSale);
        }

        public async Task DeleteSale(string id)
        {
            await _saleRepository.DeleteSale(id);
        }
    }
}
