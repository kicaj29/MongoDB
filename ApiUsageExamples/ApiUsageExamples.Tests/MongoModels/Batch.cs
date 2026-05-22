using MongoDB.Bson.Serialization.Attributes;

namespace ApiUsageExamples.Tests.MongoModels
{
    internal class Batch
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        public List<Document> Documents { get; set; } = new List<Document>();

        public DateTime LastReadAt { get; set; }

        public List<ClassDefinition> ClassDefinitions { get; set; } = new List<ClassDefinition>();
    }
}
