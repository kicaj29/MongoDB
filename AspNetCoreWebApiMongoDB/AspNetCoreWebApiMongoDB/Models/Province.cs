using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApiMongoDB.Models
{
    public class Province
    {
        [BsonId]
        public MongoDB.Bson.ObjectId _id { get; set; }

        public string ProvinceId { get; set; }
        public string Name { get; set; }
        public string CountryId { get; set; }
    }
}
