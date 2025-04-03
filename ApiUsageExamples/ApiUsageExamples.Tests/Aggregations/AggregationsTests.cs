using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiUsageExamples.Tests.Aggregations
{
    /// <summary>
    /// https://www.mongodb.com/developer/languages/csharp/handling-complex-aggregations-csharp/
    /// </summary>
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
            PipelineStageDefinition<Batch, Batch> stageUpdateLastReadAt = new BsonDocumentPipelineStageDefinition<Batch, Batch>(
                 new BsonDocument("$addFields", new BsonDocument("LastReadAt", DateTime.UtcNow)));

            GroupWithStatuses result = await collection.Aggregate()
                .Match(b => b.ID == batch.ID)
                //.AppendStage(stageUpdateLastReadAt) - AppendStage updates mongo document which exist in the pipeline but not in db
                .Unwind(b => b.Documents)
                .Group(new BsonDocument
                {
                            { "_id", BsonNull.Value },
                            { "Statuses", new BsonDocument("$addToSet", "$Documents.Status") }
                })
                .As<GroupWithStatuses>()
                .FirstOrDefaultAsync();

            // but the sam can be done also without aggregation
            List<string> statuses = await collection
                .Find(b => b.ID == batch.ID)
                .Limit(1)
                .Project(b => b.Documents.Select(d => d.Status).Distinct().ToList())
                .FirstOrDefaultAsync();

            Assert.That(statuses.Count, Is.EqualTo(3));
            // Order can be random
            Assert.That(statuses.Contains("Succeeded"), Is.True);
            Assert.That(statuses.Contains("Processing"), Is.True);
            Assert.That(statuses.Contains("Failed"), Is.True);


            // Merge operator can update collection but it must be the last stage in the pipeline so then we cannot read any data
            /*await collection.Aggregate()
                .Match(b => b.ID == batch.ID)
                //.AppendStage(stageUpdateLastReadAt) - AppendStage updates mongo document which exist in the pipeline but not in db
                .Unwind(b => b.Documents)
                .Group(new BsonDocument
                {
                                        { "_id", BsonNull.Value },
                                        { "Statuses", new BsonDocument("$addToSet", "$Documents.Status") }
                })
                .As<GroupWithStatuses>()
                .MergeAsync(
                    collection,
                    new MergeStageOptions<Batch>());*/

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Statuses.Count, Is.EqualTo(3));
            // Order can be random
            Assert.That(result.Statuses.Contains("Succeeded"), Is.True);
            Assert.That(result.Statuses.Contains("Processing"), Is.True);
            Assert.That(result.Statuses.Contains("Failed"), Is.True);

        }
    }
}
