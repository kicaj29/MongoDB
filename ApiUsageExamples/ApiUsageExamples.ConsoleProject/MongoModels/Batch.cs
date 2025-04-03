using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiUsageExamples.ConsoleProject.MongoModels
{
    internal class Batch
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        public Document[] Documents { get; set; } = default!;

        public DateTime LastReadAt { get; set; }
    }
}
