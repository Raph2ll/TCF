using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using sales.src.Models;

namespace sales.src.Models
{
    public class Sale
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? ClientId { get; set; }

        public List<SaleItem>? Items { get; set; }

        public SaleStatus Status { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedAt { get; set; }
    }
}
