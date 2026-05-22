using MongoDB.Bson.Serialization.Attributes;

namespace ApiUsageExamples.Tests.MongoModels
{
    internal class BatchUnwound
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        public Document Documents { get; set; }

        public DateTime LastReadAt { get; set; }

        public List<ClassDefinition> ClassDefinitions { get; set; } = new List<ClassDefinition>();
    }
}
