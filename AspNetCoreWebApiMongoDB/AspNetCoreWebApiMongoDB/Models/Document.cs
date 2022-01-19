using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AspNetCoreWebApiMongoDB.Models
{
    public class Document
    {
        public string ID { get; set; }

        public DocumentStatus DocumentStatus { get; set; }

        public string Status { get; set; }
    }
}
