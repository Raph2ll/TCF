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

            foreach (var item in items)
            {
                item.SellId = sale.Id; 
                await _saleItemsCollection.InsertOneAsync(item);
            }

            var updateResult = await _salesCollection.UpdateOneAsync(
                Builders<Sale>.Filter.Eq(s => s.Id, sale.Id),
                Builders<Sale>.Update.Set("Items", items)
            );
        }

        public async Task<Sale> GetSaleById(string id)
        {
            var filter = Builders<Sale>.Filter.Eq("_id", ObjectId.Parse(id));
            return await _salesCollection.Find(filter).FirstOrDefaultAsync();
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

        public async Task DeleteSale(string id)
        {
            var filter = Builders<Sale>.Filter.Eq("_id", ObjectId.Parse(id));
            await _salesCollection.DeleteOneAsync(filter);
        }
    }
}
