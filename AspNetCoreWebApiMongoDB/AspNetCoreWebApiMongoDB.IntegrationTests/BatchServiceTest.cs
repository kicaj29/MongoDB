using AspNetCoreWebApiMongoDB.Models;
using AspNetCoreWebApiMongoDB.Serializers;
using AspNetCoreWebApiMongoDB.Services;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApiMongoDB.IntegrationTests
{
    public class BatchServiceTest
    {
        private MongoConnectionString _mongoConnectionString;
        private string _batchReadyToProcessId;
        private string _batchProcessingId;
        private string _batchInVerificationIdConcurrencySet;
        private string _batchInVerificationIdConcurrencyNull;

        private void CreateCountriesAndProvincies(MongoClient client)
        {
            Country c1 = new Country()
            {
                CountryId = "1",
                CountryName = "Poland"
            };
            Country c2 = new Country()
            {
                CountryId = "2",
                CountryName = "USA"
            };

            Province p1 = new Province();
            p1.CountryId = "1";
            p1.Name = "Silesia";
            p1.ProvinceId = "1";

            IMongoDatabase db = client.GetDatabase(this._mongoConnectionString.DatabaseName);
            IMongoCollection<Country> countries = db.GetCollection<Country>("countries");
            IMongoCollection<Province> provinces = db.GetCollection<Province>("provinces");

            countries.InsertMany(new List<Country>(new Country[] { c1, c2 }));
            provinces.InsertMany(new List<Province>(new Province[] { p1 }));
        }

        // [OneTimeSetUp]
        public void OneTimeSetUp()
        {


            var connString = TestContext.Parameters.Get("mongoConnectionString");
            var databaseName = TestContext.Parameters.Get("databaseName");
            this._mongoConnectionString = new MongoConnectionString()
            {
                ConnectionString = connString,
                DatabaseName = databaseName
            };

            //generate sample data
            var client = new MongoClient(this._mongoConnectionString.ConnectionString);

            this.CreateCountriesAndProvincies(client);

            IMongoDatabase db = client.GetDatabase(this._mongoConnectionString.DatabaseName);
            IMongoCollection<Batch> batches = db.GetCollection<Batch>("batches");
            IMongoCollection<Document> documents = db.GetCollection<Document>("documents");

            batches.DeleteMany(Builders<Batch>.Filter.Empty);

            var batchReadyToProcess = new Batch();
            batchReadyToProcess.ID = Guid.NewGuid().ToString();
            batchReadyToProcess.Name = "Batch ready to process";
            //batchReadyToProcess.State = BatchState.ReadyToProcess;
            batchReadyToProcess.Suspension = BatchSuspension.None;
            batchReadyToProcess.DocumentIDs = new List<string>();

            var doc1 = new Document();
            doc1.ID = Guid.NewGuid().ToString();
            doc1.DocumentStatus = new DocumentStatus()
            {
                ActionId = "1",
                ActionType = "Type1",
                Status = "Status1"
            };
            var doc2 = new Document();
            doc2.ID = Guid.NewGuid().ToString();
            doc2.DocumentStatus = new DocumentStatus()
            {
                ActionId = "2",
                ActionType = "Type2",
                Status = "Status2"
            };
            batchReadyToProcess.DocumentIDs.Add(doc1.ID);
            batchReadyToProcess.DocumentIDs.Add(doc2.ID);

            var batchProcessing = new Batch();
            batchProcessing.ID = Guid.NewGuid().ToString();
            batchProcessing.Name = "Batch processing";
            //batchProcessing.State = BatchState.Processing;
            batchProcessing.Suspension = BatchSuspension.None;

            var batchInVerificationConcurrencySet = new Batch();
            batchInVerificationConcurrencySet.ID = Guid.NewGuid().ToString();
            batchInVerificationConcurrencySet.Name = "Batch in verification concurrency set";
            //batchInVerificationConcurrencySet.State = BatchState.Verification;
            batchInVerificationConcurrencySet.Concurrency = new Concurrency() { UserName = "kicaj29" };
            var doc3 = new Document();
            doc3.ID = Guid.NewGuid().ToString();
            doc3.DocumentStatus = new DocumentStatus()
            {
                ActionId = "3",
                ActionType = "Type3",
                Status = "Status3"
            };
            batchInVerificationConcurrencySet.DocumentIDs = new List<string>();
            batchInVerificationConcurrencySet.DocumentIDs.Add(doc3.ID);

            var batchInVerificationConcurrencyNull = new Batch();
            batchInVerificationConcurrencyNull.ID = Guid.NewGuid().ToString();
            batchInVerificationConcurrencyNull.Name = "Batch in verification concurrency null";
            // batchInVerificationConcurrencyNull.State = BatchState.Verification;
            batchInVerificationConcurrencyNull.Concurrency = null;

            batches.InsertMany(new List<Batch>(new Batch[] { batchReadyToProcess, batchProcessing, batchInVerificationConcurrencySet, batchInVerificationConcurrencyNull }));

            this._batchReadyToProcessId = batchReadyToProcess.ID;
            this._batchProcessingId = batchProcessing.ID;
            this._batchInVerificationIdConcurrencySet = batchInVerificationConcurrencySet.ID;
            this._batchInVerificationIdConcurrencyNull = batchInVerificationConcurrencyNull.ID;

            documents.InsertMany(new List<Document>(new Document[] { doc1, doc2, doc3 }));
        }



        [SetUp]
        public void Setup()
        {
            var connString = TestContext.Parameters.Get("mongoConnectionString");
            var databaseName = TestContext.Parameters.Get("databaseName");
            this._mongoConnectionString = new MongoConnectionString()
            {
                ConnectionString = connString,
                DatabaseName = databaseName
            };
        }

        [Test]
        public void TestMultipleUpdates()
        {
            var batchService = new BatchService(this._mongoConnectionString);

            var result = batchService.UpdateMultipleBatches(
                new List<string>(new string[] { this._batchInVerificationIdConcurrencySet, this._batchInVerificationIdConcurrencyNull, this._batchProcessingId, this._batchReadyToProcessId })
                );

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(1, result.Count(r => r.Id == _batchReadyToProcessId));
            Assert.AreEqual(1, result.Count(r => r.Id == _batchProcessingId));
            Assert.AreEqual(1, result.Count(r => r.Id == _batchInVerificationIdConcurrencyNull));

            Assert.AreEqual(0, result.Count(r => r.Id == _batchInVerificationIdConcurrencySet));
        }

        [Test]
        public void TestFindOneAndDelete()
        {
            var batchService = new BatchService(this._mongoConnectionString);
            var result1 = batchService.FindOneAndDelete(this._batchProcessingId);
            var result2 = batchService.FindOneAndDelete(this._batchProcessingId);

            Assert.AreEqual(this._batchProcessingId, result1.ID);
            Assert.IsNull(result2);
        }

        [Test]
        public async Task TestCustomSerializationAsync()
        {
            try
            {

                BsonClassMap.RegisterClassMap<Batch>(cm =>
                {
                    cm.AutoMap();
                    cm.MapMember(c => c.State).SetSerializer(new BatchStateV1Serializer());
                });

                var batchService = new BatchService(this._mongoConnectionString);
                var batch = await batchService.FindBatchAsync("61dd5679194033ea83d88b73");
            }
            catch(Exception ex)
            {

            }
        }

        [Test]
        public void TestMongoAggregation()
        {
            var batchService = new BatchService(this._mongoConnectionString);
            batchService.RunAggregation();
        }
    }
}