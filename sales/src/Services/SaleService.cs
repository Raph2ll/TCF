using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using sales.src.Models;
using sales.src.Models.DTOs;
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

        public async Task CreateSale(SaleRequestDTO saleRequest)
        {
            var sale = new Sale
            {
                ClientId = saleRequest.ClientId,
                Items = new List<SaleItem>(),
                Status = saleRequest.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            List<SaleItem> saleItems = new List<SaleItem>();

            foreach (var itemRequest in saleRequest.Items)
            {
                var saleItem = new SaleItem
                {
                    ProductId = itemRequest.ProductId,
                    Quantity = itemRequest.Quantity
                };

                saleItems.Add(saleItem);
            }

            await _saleRepository.CreateSale(sale, saleItems);
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
