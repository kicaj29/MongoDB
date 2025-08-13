using ApiUsageExamples.Tests.Projections;
using MongoDB.Driver;

namespace ApiUsageExamples.Tests
{
    [TestFixture]
    public class ProjectionTest : BaseTest
    {

        [Test]
        public async Task Projection_WithCountOnSubDocuments()
        {
            // Arrange
            var collection = DB.GetCollection<Batch>("Batches");
            Batch batch = new Batch();
            batch.ID = ObjectId.GenerateNewId().ToString();
            batch.LastReadAt = DateTime.UtcNow;
            batch.Documents = new Document[]
            {
                new Document { ID = ObjectId.GenerateNewId().ToString(), Status = "Processing" },
                new Document { ID = ObjectId.GenerateNewId().ToString(), Status = "Failed" },
                new Document { ID = ObjectId.GenerateNewId().ToString(), Status = "Succeeded" },
                new Document { ID = ObjectId.GenerateNewId().ToString(), Status = "Succeeded" }
            };

            await collection.InsertOneAsync(batch);
            
            // Act
            FilterDefinition<Batch> filter = Builders<Batch>.Filter.Eq(p => p.ID, batch.ID);
            ProjectionDefinition<Batch, int> projection = Builders<Batch>.Projection.Expression(b => b.Documents.Count());
            int docsCount = await collection.Find(filter).Project(projection).FirstOrDefaultAsync();

            //<7>MongoDB.Connection[0] 1 3 localhost 27017 205 Sending
            //< 7 > MongoDB.Command[0] 1 3 localhost 27017 205 8 2 Command started find ApiUsageExamples { "find" : "Batches", "filter" : { "_id" : { "$oid" : "689c437a035d7f1ae99c72a8" } }, "projection" : { "_v" : { "$size" : "$Documents" }, "_id" : 0 }, "limit" : 1, "$db" : "ApiUsageExamples", "lsid" : { "id" : { "$binary" : { "base64" : "LKhPAhUPRUaKj5IPn+R6Mw==", "subType" : "04" } } } }
            //find - { "find" : "Batches", "filter" : { "_id" : { "$oid" : "689c437a035d7f1ae99c72a8" } }, "projection" : { "_v" : { "$size" : "$Documents" }, "_id" : 0 }, "limit" : 1, "$db" : "ApiUsageExamples", "lsid" : { "id" : { "$binary" : { "base64" : "LKhPAhUPRUaKj5IPn+R6Mw==", "subType" : "04" } } } }
            //< 7 > MongoDB.Connection[0] 1 3 localhost 27017 205 Sent
            //< 7 > MongoDB.Connection[0] 1 3 localhost 27017 205 Receiving
            //< 7 > MongoDB.Command[0] 1 3 localhost 27017 205 8 2 Command succeeded find ApiUsageExamples 5.1369 { "cursor" : { "firstBatch" : [{ "_v" : 4 }], "id" : 0, "ns" : "ApiUsageExamples.Batches" }, "ok" : 1.0 }

            // Assert
            Assert.That(docsCount, Is.EqualTo(4));



            ProjectionDefinition<Batch, BatchProjectionDocsCount> projectionByClass = Builders<Batch>.Projection.Expression(b => new BatchProjectionDocsCount
            {
                DocsCount = b.Documents.Count(),
                LastReadAt = b.LastReadAt
            });
            BatchProjectionDocsCount batchProjection = await collection.Find(filter).Project(projectionByClass).FirstOrDefaultAsync();

            // <7>MongoDB.Command[0] 1 3 localhost 27017 213 8 3 Command started find ApiUsageExamples { "find" : "Batches", "filter" : { "_id" : { "$oid" : "689c47e2580d78db517714c4" } }, "projection" : { "DocsCount" : { "$size" : "$Documents" }, "LastReadAt" : 1, "_id" : 0 }, "limit" : 1, "$db" : "ApiUsageExamples", "lsid" : { "id" : { "$binary" : { "base64" : "T8sZsFVBRaOtYyY2wuS+2A==", "subType" : "04" } } } }
            // find - { "find" : "Batches", "filter" : { "_id" : { "$oid" : "689c47e2580d78db517714c4" } }, "projection" : { "DocsCount" : { "$size" : "$Documents" }, "LastReadAt" : 1, "_id" : 0 }, "limit" : 1, "$db" : "ApiUsageExamples", "lsid" : { "id" : { "$binary" : { "base64" : "T8sZsFVBRaOtYyY2wuS+2A==", "subType" : "04" } } } }
            // < 7 > MongoDB.Connection[0] 1 3 localhost 27017 213 Sent
            // < 7 > MongoDB.Connection[0] 1 3 localhost 27017 213 Receiving
            // < 7 > MongoDB.Command[0] 1 3 localhost 27017 213 8 3 Command succeeded find ApiUsageExamples 1.3923 { "cursor" : { "firstBatch" : [{ "LastReadAt" : { "$date" : { "$numberLong" : "-62135596800000" } }, "DocsCount" : 4 }], "id" : 0, "ns" : "ApiUsageExamples.Batches" }, "ok" : 1.0 }

            // Assert
            Assert.That(batchProjection.DocsCount, Is.EqualTo(4));
            Assert.That(batchProjection.LastReadAt, Is.InRange(DateTime.UtcNow.AddSeconds(-10), DateTime.UtcNow));

        }
    }
}
