using System;
using MongoDB.Bson;
using sales.src.Models;

namespace sales.src.Repositories.Interfaces
{
    public interface ISaleRepository
    {
        public Task CreateSale(Sale sale);
        public Task AddItemsToSale(string id, List<SaleItem> items);
        public Task<Sale> GetSaleById(string id);
        public Task<Sale> GetSaleByClientId(string clientId);
        public Task<List<Sale>> GetAllSales();
        public Task UpdateSale(string id, Sale updatedSale);
        public Task ConfirmSale(string saleId);
        public Task RemoveItemsFromSale(string saleId, List<string> itemIds);
        public Task DeleteSale(string id);
    }
}