
using ApiUsageExamples.ConsoleProject.MongoModels;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using System.Diagnostics;


namespace ApiUsageExamples.ConsoleProject
{
    public class StorageService
    {
        public IMongoDatabase DB { get; set; }
        public IMongoDatabase AdminDB { get; set; }

        public MongoClientSettings MongoClientSettings { get; set; }

        public StorageService()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            var connString = "mongodb://localhost:27017";
            var dbName = "ApiUsageExamples";

            var mongoConnectionUrl = new MongoUrl(connString);
            MongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);

            var loggerFactory = LoggerFactory.Create(b =>
            {
                b.SetMinimumLevel(LogLevel.Trace);
                b.AddSimpleConsole();
                b.AddSystemdConsole();
                b.AddConsole();
            });

            MongoClientSettings.LoggingSettings = new MongoDB.Driver.Core.Configuration.LoggingSettings(loggerFactory);

            // http://mongodb.github.io/mongo-csharp-driver/2.8/apidocs/html/N_MongoDB_Driver_Core_Events.htm
            MongoClientSettings.ClusterConfigurator = cb =>
            {
                cb.Subscribe<CommandStartedEvent>(e =>
                {
                    Console.WriteLine($"{e.CommandName} - {e.Command.ToJson()}");
                });

            };

            // MongoClient must be created after subscribing to the events
            var client = new MongoClient(MongoClientSettings);
            DB = client.GetDatabase(dbName);
            AdminDB = client.GetDatabase("admin");
        }


        public async Task FindOneAndUpdateWithProjection()
        {
            // Arrange
            var collection = DB.GetCollection<Batch>("Batches");
            Batch batch = new Batch();
            batch.ID = ObjectId.GenerateNewId().ToString();
            Console.WriteLine($"Generated batch id: {batch.ID}");
            List<Document> documents = new List<MongoModels.Document>();
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

            FilterDefinition<Batch> filter = Builders<Batch>.Filter.Eq(p => p.ID, batch.ID);
            UpdateDefinition<Batch> update = Builders<Batch>.Update.Set(b => b.LastReadAt, DateTime.UtcNow);
            ProjectionDefinition<Batch, List<string>> projection = Builders<Batch>.Projection.Expression(b => b.Documents.Select(d => d.Status).Distinct().ToList());

            // Clear console to focus only on logs from FindOneAndUpdateAsync
            Console.Clear();

            List<string> statuses = await collection.FindOneAndUpdateAsync(filter, update,
                new FindOneAndUpdateOptions<Batch, List<string>>()
                {
                    ReturnDocument = ReturnDocument.Before,
                    Projection = projection
                });
        }
    }
}
