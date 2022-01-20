using AspNetCoreWebApiMongoDB.Models;
using AspNetCoreWebApiMongoDB.Models.AggregationTempModels;
using MongoDB.Bson;
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
                .Where(b => ids.Contains(b.ID) && b.Suspension == BatchSuspension.None && b.Concurrency == null);


            var update = Builders<Batch>.Update
                .Set(p => p.Suspension, BatchSuspension.Suspending);

            batches.UpdateMany(filter, update);

            var filterForCheck = Builders<Batch>.Filter
                .Where(b => ids.Contains(b.ID) && b.Suspension == BatchSuspension.Suspending);


            var updatedBatches = batches.Find(filterForCheck).ToList();

            foreach(var b in updatedBatches)
            {
                result.Add(new UpdateResultForId()
                {
                    Id = b.ID,
                    Result = UpdateResult.Suspending
                });
            }

            return result;
        }

        public Batch FindOneAndDelete(string id)
        {
            IMongoCollection<Batch> batches = this._db.GetCollection<Batch>("batches");

            return batches.FindOneAndDelete(Builders<Batch>.Filter.Where(b => b.ID == id));
        }

        public async Task<Batch> FindBatchAsync(string id)
        {
            IMongoCollection<Batch> batches = this._db.GetCollection<Batch>("batches");
            using (var cursor = await batches.FindAsync(Builders<Batch>.Filter.Where(b => b.ID == id), new FindOptions<Batch>() { Limit = 1 }))
            {
                return await cursor.FirstOrDefaultAsync();
            }
        }

        private void SimpleLinq()
        {

            // this is just pure linq in c#
            Batch b = new Batch();
            b.ID = "batch1";
            b.DocumentIDs = new List<string>(new string[] { "1" });

            Document d = new Document();
            d.ID = "1";

            List<Batch> batches = new List<Batch>(new Batch[] { b } );
            List<Document> documents = new List<Document>(new Document[] { d });

            var batchesQuerable = batches.AsQueryable();
            var documentsQuerable = documents.AsQueryable();

            var query = documentsQuerable.Where(d => batchesQuerable.Where(b => b.ID == "batch1" && b.DocumentIDs.Contains(d.ID)).Any());
            List<Document> docs = query.ToList();

        }

        private void SimpleAggregation()
        {
            // https://www.youtube.com/watch?v=P0vDBxlFA-k
            IMongoCollection<Country> countries = this._db.GetCollection<Country>("countries");
            IMongoCollection<Province> provinces = this._db.GetCollection<Province>("provinces");

            var result = countries.Aggregate()
                .Lookup<Country, Province, CountryLookedUp>(provinces, c => c.CountryId,
                    p => p.ProvinceId, c => c.ProvinceList)
                .ToList();
        }

        private void SimpleAggregationV2()
        {
            // https://www.youtube.com/watch?v=P0vDBxlFA-k
            IMongoCollection<Country> countries = this._db.GetCollection<Country>("countries");
            IMongoCollection<Province> provinces = this._db.GetCollection<Province>("provinces");

            var result = countries.Aggregate()
                .Lookup<Country, Province, CountryLookedUp>(provinces, c => c.CountryId,
                    p => p.ProvinceId, lu => lu.ProvinceList)
                .ToEnumerable()
                .SelectMany(lu => lu.ProvinceList.Select(
                    p => new
                    {
                        p.Name
                    }
                    )
                )
                .ToList();
        }

        public async Task RunAggregation()
        {
            //this.SimpleLinq();
            //this.SimpleAggregation();
            //this.SimpleAggregationV2();

            IMongoCollection<Batch> batches = this._db.GetCollection<Batch>("batches");
            IMongoCollection<Document> documents = this._db.GetCollection<Document>("documents");

            var result = batches.Aggregate()
                .Match(b => b.ID == "c4de2759-6580-42e6-a3e5-aabd9cf3fc8b")
                .Unwind(b => b.DocumentIDs)
                .ToList();
            var docId = result[0].GetValue("DocumentIDs").AsString;

            try
            {
                var result2 = await batches.Aggregate()
                    .Match(b => b.ID == "c4de2759-6580-42e6-a3e5-aabd9cf3fc8b")
                    .Lookup<Document, Batch>("documents", nameof(Batch.DocumentIDs), nameof(Document.ID), "DocumentData")
                    .Project($"{{ \"DocumentData.{nameof(Document.DocumentStatus)}\": 1 }}")
                    .Unwind("DocumentData")
                    .Project($"{{ status: \"$DocumentData.{nameof(Document.DocumentStatus)}.{nameof(DocumentStatus.Status)}\"," +
                             $" actionType: \"$DocumentData.{nameof(Document.DocumentStatus)}.{nameof(DocumentStatus.ActionType)}\"," +
                             $"_id: 0 }}"
                        )
                    .ToListAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
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
