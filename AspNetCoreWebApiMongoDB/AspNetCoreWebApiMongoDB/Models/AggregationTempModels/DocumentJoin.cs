using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApiMongoDB.Models.AggregationTempModels
{
    public class DocumentJoin
    {
        public string _id { get; set; }
        public List<Document> Documents { get; set; }
    }
}
