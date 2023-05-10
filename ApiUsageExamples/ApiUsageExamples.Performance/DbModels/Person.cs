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

        public List<string> Data1 { get; set; } = default!;

        public List<string> Data2 { get; set; } = default!;

        public List<string> Data3 { get; set; } = default!;

        public List<string> Data4 { get; set; } = default!;

    }
}
