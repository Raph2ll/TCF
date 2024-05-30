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
                    throw new BadRequestException($"Product '{product.Name}':{product.Id} does not have enough quantity available.");
                }

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
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                throw new ArgumentException("Invalid ID format", nameof(id));
            }

            var sale = await _saleRepository.GetSaleById(objectId.ToString());
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

        public async Task ConfirmSale(string saleId)
        {
            var sale = await GetSaleById(saleId);
            if (sale.Status == SaleStatus.STARTED)
            {
                throw new BadRequestException("This sale don't have any item.");
            }
            if (sale.Status == SaleStatus.DONE)
            {
                throw new BadRequestException("This sale is already confirmed.");
            }

            foreach (var item in sale.Items)
            {
                var productResponse = await _productApi.GetProductById(item.ProductId);
                var product = productResponse.Content;

                if (product.Quantity < item.Quantity)
                {
                    throw new BadRequestException($"Product '{product.Name}':{product.Id} does not have enough quantity available.");
                }

                var quantityDecrease = product.Quantity - item.Quantity;

                var updateRequest = new ProductUpdateRequest
                {
                    Quantity = quantityDecrease
                };

                await _productApi.UpdateProduct(item.ProductId, updateRequest);
            }

            await _saleRepository.ConfirmSale(saleId);
        }

        public async Task UpdateItemsFromSale(string saleId, List<SaleItemRequestDTO> itemUpdates)
        {
            var sale = await GetSaleById(saleId);
            if (sale.Status == SaleStatus.STARTED)
            {
                throw new BadRequestException("This sale doesn't have any item.");
            }

            foreach (var itemUpdate in itemUpdates)
            {
                var saleItem = sale.Items.FirstOrDefault(item => item.Id == itemUpdate.ProductId);
                if (saleItem == null)
                {
                    throw new NotFoundException($"Item with Id '{itemUpdate.ProductId}' not found in sale with Id '{saleId}'.");
                }

                // Se a venda está concluída, atualiza a quantidade no estoque
                if (sale.Status == SaleStatus.DONE)
                {
                    var productResponse = await _productApi.GetProductById(saleItem.ProductId);
                    var product = productResponse.Content;

                    // Calcula a diferença na quantidade e atualiza o estoque
                    int quantityDifference = itemUpdate.Quantity - saleItem.Quantity;
                    product.Quantity += quantityDifference;

                    var updateRequest = new ProductUpdateRequest { Quantity = product.Quantity };
                    var updateResponse = await _productApi.UpdateProduct(saleItem.ProductId, updateRequest);

                    if (!updateResponse.IsSuccessStatusCode)
                    {
                        throw new Exception($"Failed to update quantity for product '{product.Name}':{product.Id}.");
                    }
                }

                // Atualiza a quantidade do item na venda
                saleItem.Quantity = itemUpdate.Quantity;
            }

            // Atualiza a venda no repositório
            await _saleRepository.UpdateSaleItems(saleId, sale.Items);
        }

        public async Task RemoveItemsFromSale(string saleId, List<string> itemIds)
        {
            var sale = await GetSaleById(saleId);
            if (sale.Status == SaleStatus.STARTED)
            {
                throw new BadRequestException("This sale don't have any item.");
            }

            foreach (var itemId in itemIds)
            {
                if (!sale.Items.Any(item => item.Id == itemId))
                {
                    throw new NotFoundException($"Item with Id '{itemId}' not found in sale with Id '{saleId}'.");
                }
                if (sale.Status == SaleStatus.DONE)
                {
                    var productResponse = await _productApi.GetProductById(itemId);
                    var product = productResponse.Content;

                    var item = sale.Items.FirstOrDefault(i => i.Id == itemId);
                    var quantityIncrease = product.Quantity + item.Quantity;

                    var updateRequest = new ProductUpdateRequest { Quantity = quantityIncrease };

                    await _productApi.UpdateProduct(item.ProductId, updateRequest);
                }
            }

            await _saleRepository.RemoveItemsFromSale(saleId, itemIds);
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
