using AspNetCoreWebApiMongoDB.Models;
using AspNetCoreWebApiMongoDB.Services;
using MongoDB.Driver;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreWebApiMongoDB.IntegrationTests
{
    public class BatchServiceTest
    {
        private MongoConnectionString _mongoConnectionString;
        private string _batchReadyToProcessId;
        private string _batchProcessingId;
        private string _batchInVerificationIdConcurrencySet;
        private string _batchInVerificationIdConcurrencyNull;

        [OneTimeSetUp]
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
            IMongoDatabase db = client.GetDatabase(this._mongoConnectionString.DatabaseName);
            IMongoCollection<Batch> batches = db.GetCollection<Batch>("batches");

            batches.DeleteMany(Builders<Batch>.Filter.Empty);

            var batchReadyToProcess = new Batch();
            batchReadyToProcess.Name = "Batch ready to process";
            batchReadyToProcess.State = BatchState.ReadyToProcess;
            batchReadyToProcess.Suspension = BatchSuspension.None;

            var batchProcessing = new Batch();
            batchProcessing.Name = "Batch processing";
            batchProcessing.State = BatchState.Processing;
            batchProcessing.Suspension = BatchSuspension.None;

            var batchInVerificationConcurrencySet = new Batch();
            batchInVerificationConcurrencySet.Name = "Batch in verification concurrency set";
            batchInVerificationConcurrencySet.State = BatchState.Verification;
            batchInVerificationConcurrencySet.Concurrency = new Concurrency() { UserName = "kicaj29" };


            var batchInVerificationConcurrencyNull = new Batch();
            batchInVerificationConcurrencyNull.Name = "Batch in verification concurrency null";
            batchInVerificationConcurrencyNull.State = BatchState.Verification;
            batchInVerificationConcurrencyNull.Concurrency = null;

            batches.InsertMany(new List<Batch>(new Batch[] { batchReadyToProcess, batchProcessing, batchInVerificationConcurrencySet, batchInVerificationConcurrencyNull }));

            this._batchReadyToProcessId = batchReadyToProcess.Id;
            this._batchProcessingId = batchProcessing.Id;
            this._batchInVerificationIdConcurrencySet = batchInVerificationConcurrencySet.Id;
            this._batchInVerificationIdConcurrencyNull = batchInVerificationConcurrencyNull.Id;
        }



        [SetUp]
        public void Setup()
        {

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

            Assert.AreEqual(this._batchProcessingId, result1.Id);
            Assert.IsNull(result2);
        }
    }
}