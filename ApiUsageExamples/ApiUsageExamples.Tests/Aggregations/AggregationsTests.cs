using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiUsageExamples.Tests.Aggregations
{
    internal class AggregationsTests : BaseTest
    {
        [Test]
        public async Task SingleCollectionWithSubArray_SwitchCase()
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
                Status = "Processing"
            });
            var doc2Id = ObjectId.GenerateNewId().ToString();
            documents.Add(new Document()
            {
                ID = doc2Id,
                Status = "Failed"
            });
            var doc3Id = ObjectId.GenerateNewId().ToString();
            documents.Add(new Document()
            {
                ID = doc3Id,
                Status = "Succeeded"
            });
            var doc4Id = ObjectId.GenerateNewId().ToString();
            documents.Add(new Document()
            {
                ID = doc4Id,
                Status = "Succeeded"
            });
            batch.Documents = documents.ToArray();
            await collection.InsertOneAsync(batch);

            Batch batch2 = new Batch();
            batch2.ID = ObjectId.GenerateNewId().ToString();
            Console.WriteLine($"Generated batch id: {batch2.ID}");
            List<Document> documents2 = new List<Document>();
            var doc5Id = ObjectId.GenerateNewId().ToString();
            documents2.Add(new Document()
            {
                ID = doc5Id,
                Status = "Processing"
            });
            batch2.Documents = documents2.ToArray();
            await collection.InsertOneAsync(batch2);

            // Act
            var result = await collection.Aggregate()
                .Match(b => b.ID == batch.ID)
                .Unwind(b => b.Documents)
                .Group(new BsonDocument
                {
                        { "_id", BsonNull.Value },
                        { "Statuses", new BsonDocument("$addToSet", "$Documents.Status") }
                })
                .FirstOrDefaultAsync();


            Assert.AreEqual(3, groupWithStatuses.Statuses.Count);
            Assert.Contains("Processing", groupWithStatuses.Statuses);
            Assert.Contains("Failed", groupWithStatuses.Statuses);
            Assert.Contains("Succeeded", groupWithStatuses.Statuses);
        }
    }
}
