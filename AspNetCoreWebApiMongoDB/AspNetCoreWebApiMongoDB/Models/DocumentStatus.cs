using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApiMongoDB.Models
{
    public class DocumentStatus
    {
        public string ActionId { get; set; }
        public string ActionType { get; set; }
        public string Status { get; set; }
    }
}
