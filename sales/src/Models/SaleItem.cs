using MongoDB.Bson.Serialization.Attributes;

namespace sales.src.Models
{
    public class SaleItem
    {
        public string? SellId { get; set; }

        public string? ProductId { get; set; }

        public int Quantity { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedAt { get; set; }
    }
}
