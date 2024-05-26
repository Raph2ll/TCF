using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using sales.src.Models;
using sales.src.Models.DTOs;
using sales.src.Repositories.Interfaces;
using sales.src.Services.Interfaces;
using sales.src.Services.Refit;
using sales.src.Exceptions;
using MongoDB.Bson;

namespace sales.src.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IClient _clientApi;
        private readonly IProduct _productApi;

        public SaleService(ISaleRepository saleRepository, IClient clientApi, IProduct productApi)
        {
            _saleRepository = saleRepository;
            _clientApi = clientApi;
            _productApi = productApi;
        }

        public async Task CreateSale(string id)
        {
            var clientResponse = await _clientApi.GetClientById(id);
            if (clientResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new NotFoundException($"Client with Id '{id}' not found.");
            }

            var existingSale = await _saleRepository.GetSaleByClientId(id);
            if (existingSale != null)
            {
                throw new BadRequestException($"There is already a sale with ClientId '{id}'.");
            }

            var sale = new Sale
            {
                ClientId = id,
                Items = new List<SaleItem>(),
                Status = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _saleRepository.CreateSale(sale);
        }

        public async Task AddItemsToSale(string id, List<SaleItemRequestDTO> saleRequest)
        {
            var sale = await GetSaleById(id);

            var existingProductIds = new HashSet<string>(sale.Items.Select(i => i.ProductId));
            List<SaleItem> saleItems = new List<SaleItem>();

            int index = 0;
            foreach (var itemRequest in saleRequest)
            {
                if (!existingProductIds.Add(itemRequest.ProductId))
                {
                    throw new BadRequestException($"Item with ProductId '{itemRequest.ProductId}' is duplicated in the request or already exists in the sale.");
                }

                var productResponse = await _productApi.GetProductById(itemRequest.ProductId);

                if (productResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new NotFoundException($"Item[{index}] with ProductId '{itemRequest.ProductId}' not found.");
                }

                var product = productResponse.Content;
                if (product.Quantity < itemRequest.Quantity)
                {
                    throw new BadRequestException($"Product '{product.Name}' does not have enough quantity available.");
                }

                var quantityDecrease = product.Quantity - itemRequest.Quantity;

                await UpdateProductQuantity(itemRequest.ProductId, quantityDecrease);

                var saleItem = new SaleItem
                {
                    ProductId = itemRequest.ProductId,
                    Quantity = itemRequest.Quantity
                };

                saleItems.Add(saleItem);
                index++;
            }
            await _saleRepository.AddItemsToSale(id, saleItems);
        }
        public async Task<Sale> GetSaleById(string id)
        {
            var sale = await _saleRepository.GetSaleById(id);
            if (sale == null)
            {
                throw new NotFoundException($"Sale with Id '{id}' not found.");
            }

            return sale;
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

        public async Task<Product> UpdateProductQuantity(string id, int newQuantity)
        {
            var request = new ProductUpdateRequest { Quantity = newQuantity };
            var response = await _productApi.UpdateProduct(id, request);

            if (response.IsSuccessStatusCode)
            {
                return response.Content;
            }
            else
            {
                throw new BadRequestException($"Failed to update product quantity.");
            }
        }

        public async Task DeleteSale(string id)
        {
            var sale = await GetSaleById(id);

            int index = 0;
            foreach (var item in sale.Items)
            {
                var productResponse = await _productApi.GetProductById(item.ProductId);
                if (productResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new NotFoundException($"Item[{index}] with ProductId '{item.ProductId}' not found.");
                }

                var product = productResponse.Content;

                var updatedQuantity = product.Quantity + item.Quantity;
                await UpdateProductQuantity(item.ProductId, updatedQuantity);
            }

            await _saleRepository.DeleteSale(id);
        }
    }
}
