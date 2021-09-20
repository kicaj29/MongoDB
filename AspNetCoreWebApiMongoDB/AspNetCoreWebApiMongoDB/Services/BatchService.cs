using AspNetCoreWebApiMongoDB.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApiMongoDB.Services
{
    public class BatchService
    {
        private readonly IMongoDatabase _db;
        private readonly IMongoCollection<Batch> _batches;

        public BatchService(MongoConnectionString connString)
        {
            var client = new MongoClient(connString.ConnectionString);
            this._db = client.GetDatabase(connString.DatabaseName);
        }
    }
}
