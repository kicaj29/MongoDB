using AspNetCoreWebApiMongoDB.Models;
using AspNetCoreWebApiMongoDB.Services;
using MongoDB.Driver;
using NUnit.Framework;
using System.Collections.Generic;

namespace AspNetCoreWebApiMongoDB.IntegrationTests
{
    public class BatchServiceTest
    {
        private MongoConnectionString _mongoConnectionString;
        private string _batchReadyToProcessId;
        private string _batchProcessingId;
        private string _batchInVerificationId;

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
            batchReadyToProcess = new Batch();
            batchReadyToProcess.Name = "Batch ready to process";
            batchReadyToProcess.State = BatchState.ReadyToProcess;
            batchReadyToProcess.Suspension = BatchSuspension.None;

            var batchProcessing = new Batch();
            batchProcessing = new Batch();
            batchProcessing.Name = "Batch processing";
            batchProcessing.State = BatchState.Processing;
            batchProcessing.Suspension = BatchSuspension.None;

            var batchInVerification = new Batch();
            batchInVerification = new Batch();
            batchInVerification.Name = "Batch in verification";
            batchInVerification.State = BatchState.Verification;
            batchInVerification.Concurrency = new Concurrency() { UserName = "kicaj29" };

            batches.InsertMany(new List<Batch>(new Batch[] { batchReadyToProcess, batchProcessing, batchInVerification }));

            this._batchReadyToProcessId = batchReadyToProcess.Id;
            this._batchProcessingId = batchProcessing.Id;
            this._batchInVerificationId = batchInVerification.Id;
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
                new List<string>(new string[] { this._batchInVerificationId, this._batchProcessingId, this._batchReadyToProcessId })
                );

            Assert.AreEqual(2, result.Count);
        }
    }
}