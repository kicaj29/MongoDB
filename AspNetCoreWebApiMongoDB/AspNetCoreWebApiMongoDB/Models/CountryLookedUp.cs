using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApiMongoDB.Models
{
    public class CountryLookedUp : Country
    {
        public List<Province> ProvinceList { get; set; }
    }
}
