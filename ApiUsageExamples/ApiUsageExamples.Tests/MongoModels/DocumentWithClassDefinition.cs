using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiUsageExamples.Tests.MongoModels
{
    internal class DocumentWithClassDefinition
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string DocId { get; set; } = string.Empty;
        public string ClassId { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
    }
}
