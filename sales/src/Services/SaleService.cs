using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using sales.src.Models;
using sales.src.Models.DTOs;
using sales.src.Repositories.Interfaces;
using sales.src.Services.Interfaces;
using sales.src.Services.Refit;
using sales.src.Exceptions;

namespace sales.src.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IClient _clientApi;

        public SaleService(ISaleRepository saleRepository, IClient clientApi)
        {
            _saleRepository = saleRepository;
            _clientApi = clientApi;
        }

        public async Task CreateSale(SaleRequestDTO saleRequest)
        {
            var clientResponse = await _clientApi.GetClientById(saleRequest.ClientId);
            if (clientResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new NotFoundException($"Client with Id '{saleRequest.ClientId}' not found.");
            }

            var sale = new Sale
            {
                ClientId = saleRequest.ClientId,
                Items = new List<SaleItem>(),
                Status = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _saleRepository.CreateSale(sale);
        }

        public async Task AddItemsToSale(string id, List<SaleItemRequestDTO> saleRequest)
        {
            var sale = await _saleRepository.GetSaleById(id);
            if (sale == null)
            {
                throw new NotFoundException($"Sale with Id '{id}' not found.");
            }
            
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
