using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApiMongoDB.Models
{
    public class CrewNoId
    {
        [BsonElement("name")]
        public string CrewName { get; set; }

        [BsonElement("skills")]
        public List<string> Skills { get; set; }

        [BsonElement("address")]
        public Address Address { get; set; }
    }
}
