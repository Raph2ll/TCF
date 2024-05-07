using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace sales.src.Models
{
    public class SaleItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? SellId { get; set; }

        public string? ProductId { get; set; }

        public int Quantity { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedAt { get; set; }
    }
}
