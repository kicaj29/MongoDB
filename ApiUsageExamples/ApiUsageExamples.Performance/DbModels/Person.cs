using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ApiUsageExamples.Performance.DbModels
{
    public class Person
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
    }
}
