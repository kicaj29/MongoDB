using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiUsageExamples.ConsoleProject.MongoModels
{
    internal class Document
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        public string Status { get; set; } = string.Empty;
        public ActionStatus[] Actions { get; set; } = default!;
    }
}
