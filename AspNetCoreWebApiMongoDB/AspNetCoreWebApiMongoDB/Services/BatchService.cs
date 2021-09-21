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


        public List<UpdateResultForId> UpdateMultipleBatches(IEnumerable<string> ids)
        {

            var result = new List<UpdateResultForId>();

            IMongoCollection<Batch> batches = this._db.GetCollection<Batch>("batches");

            var filter = Builders<Batch>.Filter
                .Where(b => ids.Contains(b.Id) && b.Suspension == BatchSuspension.None && b.Concurrency == null);


            var update = Builders<Batch>.Update
                .Set(p => p.Suspension, BatchSuspension.Suspending);

            batches.UpdateMany(filter, update);

            var filterForCheck = Builders<Batch>.Filter
                .Where(b => ids.Contains(b.Id) && b.Suspension == BatchSuspension.Suspending);


            var updatedBatches = batches.Find(filterForCheck).ToList();

            foreach(var b in updatedBatches)
            {
                result.Add(new UpdateResultForId()
                {
                    Id = b.Id,
                    Result = UpdateResult.Suspending
                });
            }

            return result;

        }
    }

    public class UpdateResultForId
    {
        public string Id { get; set; }
        public UpdateResult Result { get; set; }
    }

    public enum UpdateResult
    {
        Updated = 0,
        InVerificationRejected = 1,
        NotFound = 2,
        Deleted = 3,
        Suspending = 4
    }
}
