using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApiMongoDB.Models
{
    public class Country
    {
        [BsonId]
        public MongoDB.Bson.ObjectId _id { get; set; }

        public string CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
