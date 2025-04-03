using MongoDB.Bson.Serialization.Attributes;

namespace ApiUsageExamples.Tests.MongoModels
{
    public class GroupWithStatuses
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ID { get; set; }

        public List<string> Statuses { get; set; } = new List<string>();
    }
}
