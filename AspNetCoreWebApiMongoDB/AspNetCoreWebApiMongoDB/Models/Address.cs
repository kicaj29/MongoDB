using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApiMongoDB.Models
{
    public class Address
    {
        [BsonElement("city")]
        public string City { get; set; }
        [BsonElement("country")]
        public string Country { get; set; }
    }
}
