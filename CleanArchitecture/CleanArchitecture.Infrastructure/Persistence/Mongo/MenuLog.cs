using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


namespace CleanArchitecture.Infrastructure.Persistence.Mongo
{
    public class MenuLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public int MenuId { get; set; }

        public string MenuName { get; set; } = null!;

        public string? Slug { get; set; }

        public int? DisplayOrder { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
