using ApiUsageExamples.Tests.MongoModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiUsageExamples.Tests
{
    internal class FindOneAndUpdateTest : BaseTest
    {

        /// <summary>
        /// This test shows that ReturnDocument.After returns document with changes executed only
        /// by the current update operation, changes executed by other threads are out of scope.
        /// </summary>
        /// <returns></returns>
        [Test]
        [Repeat(10)]
        public async Task FindOneAndUpdateMultithreading()
        {
            // Arrange
            var collection = DB.GetCollection<Batch>("Batches");
            Batch batch = new Batch();
            batch.ID = ObjectId.GenerateNewId().ToString();
            Console.WriteLine($"Generated batch id: {batch.ID}");
            List<Document> documents = new List<Document>();
            var doc1Id = ObjectId.GenerateNewId().ToString();
            documents.Add(new Document()
            {
                ID = doc1Id,
                Actions = new ActionStatus[] { new ActionStatus() { Status = "Status_0", StatusProgressCounter = 0 } }
            });
            var doc2Id = ObjectId.GenerateNewId().ToString();
            documents.Add(new Document()
            {
                ID = doc2Id,
                Actions = new ActionStatus[] { new ActionStatus() { Status = "Status_0", StatusProgressCounter = 0 } }
            });
            batch.Documents = documents.ToArray();
            await collection.InsertOneAsync(batch);

            // Act

            int maxCounter = 500;
            Task[] allTasks = new Task[maxCounter];

            for (int index = 0; index < maxCounter; index++)
            {
                allTasks[index] = TryUpdateBatch(index, batch);
            }

            await Task.WhenAll(allTasks);

            await collection.DeleteOneAsync(Builders<Batch>.Filter.Eq(p => p.ID, batch.ID));

        }

        private async Task TryUpdateBatch(int currentCounterValue, Batch batch)
        {
            await Task.Delay(100);
            Console.WriteLine($"Starting task with current counter value {currentCounterValue}.");
            var collection = DB.GetCollection<Batch>("Batches");
            FilterDefinition<Batch> filter = Builders<Batch>.Filter.Eq(p => p.ID, batch.ID);
            BsonDocument documentElement = new()
                {
                        { "documentElement.ID", new BsonDocument
                            {
                                // convert to ObjectId to have correct type in BsonArray
                                { "$in", new BsonArray(new ObjectId[] { ObjectId.Parse(batch.Documents[0].ID), ObjectId.Parse(batch.Documents[1].ID) }) }
                            }
                        },
                };


            BsonDocument documentActionElement = new()
                    {
                        { "documentActionElement.Status", $"Status_{currentCounterValue}" },
                    };

            List<ArrayFilterDefinition> arrayFilters = new()
                    {
                        new BsonDocumentArrayFilterDefinition<Batch>(documentElement),
                        new BsonDocumentArrayFilterDefinition<Batch>(documentActionElement),
                    };

            int nextCounterValue = currentCounterValue + 1;
            string expectedStatus = $"Status_{nextCounterValue}";
            UpdateDefinition<Batch> updateDefinition = Builders<Batch>.Update
                .Set("Documents.$[documentElement].Actions.$[documentActionElement].Status", expectedStatus)
                .Inc("Documents.$[documentElement].Actions.$[documentActionElement].StatusProgressCounter", 1);

            int retry = 0;
            bool success = false;
            do
            {

                var updateBatch = await collection.FindOneAndUpdateAsync(filter, updateDefinition,
                   new FindOneAndUpdateOptions<Batch, Batch>()
                   {
                       ReturnDocument = ReturnDocument.After,
                       ArrayFilters = arrayFilters
                   });

                success = updateBatch.Documents[1].Actions[0].StatusProgressCounter == nextCounterValue;
                if (success)
                    break;

                bool counterValueExceeded = updateBatch.Documents[1].Actions[0].StatusProgressCounter > nextCounterValue;
                if (counterValueExceeded)
                {
                    Assert.Fail($"Expected counter value: {nextCounterValue}, actual counter value: {updateBatch.Documents[1].Actions[0].StatusProgressCounter},\n" +
                        $"Expected status: {expectedStatus}, actual status: {updateBatch.Documents[1].Actions[0].Status} ");
                }

                retry++;
                // await Task.Delay(10);
            }
            while (retry < 500);
            if (!success)
                Console.WriteLine("FAILED to update counter");
        }
    }
}
