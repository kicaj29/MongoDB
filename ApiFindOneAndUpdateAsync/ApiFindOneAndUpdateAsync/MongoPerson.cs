using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiFindOneAndUpdateAsync
{
    public class MongoPerson
    {
        [BsonId]
        public MongoDB.Bson.ObjectId Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
