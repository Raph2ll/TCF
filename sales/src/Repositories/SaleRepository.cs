using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using sales.src.Models;
using sales.src.Repositories.Interfaces;

namespace sales.src.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly IMongoCollection<Sale> _salesCollection;
        private readonly IMongoCollection<SaleItem> _saleItemsCollection;

        public SaleRepository(IMongoDatabase database)
        {
            _salesCollection = database.GetCollection<Sale>("sales");
            _saleItemsCollection = database.GetCollection<SaleItem>("sale_items");
        }

        public async Task CreateSale(Sale sale)
        {
            await _salesCollection.InsertOneAsync(sale);
        }

        public async Task AddItemsToSale(string id, List<SaleItem> items)
        {
            foreach (var item in items)
            {
                item.SellId = id;
                await _saleItemsCollection.InsertOneAsync(item);
            }

            var updateResult = await _salesCollection.UpdateOneAsync(
                Builders<Sale>.Filter.Eq(s => s.Id, id),
                Builders<Sale>.Update.Combine(
                    Builders<Sale>.Update.PushEach(s => s.Items, items),
                    Builders<Sale>.Update.Set("Status", 1)
                )
            );
        }

        public async Task<Sale> GetSaleById(string id)
        {
            var filter = Builders<Sale>.Filter.Eq(s => s.Id, id);
            var sale = await _salesCollection.Find(filter).FirstOrDefaultAsync();
            return sale;
        }

        public async Task<Sale> GetSaleByClientId(string clientId)
        {
            var filter = Builders<Sale>.Filter.Eq(s => s.ClientId, clientId);
            var sale = await _salesCollection.Find(filter).FirstOrDefaultAsync();
            return sale;
        }

        public async Task<List<Sale>> GetAllSales()
        {
            return await _salesCollection.Find(_ => true).ToListAsync();
        }

        public async Task UpdateSale(string id, Sale updatedSale)
        {
            updatedSale.UpdatedAt = DateTime.UtcNow;
            var filter = Builders<Sale>.Filter.Eq("_id", ObjectId.Parse(id));
            await _salesCollection.ReplaceOneAsync(filter, updatedSale);
        }

        public async Task ConfirmSale(string saleId)
        {
            await _salesCollection.UpdateOneAsync(
                 Builders<Sale>.Filter.Eq(s => s.Id, saleId),
                 Builders<Sale>.Update
                     .Set("Status", 2)
                     .Set(s => s.UpdatedAt, DateTime.UtcNow));
        }

        public async Task DeleteSale(string id)
        {
            await _salesCollection.UpdateOneAsync(
                Builders<Sale>.Filter.Eq("_id", ObjectId.Parse(id)),
                Builders<Sale>.Update
                        .Set("Status", 3)
                );
        }
    }
}
