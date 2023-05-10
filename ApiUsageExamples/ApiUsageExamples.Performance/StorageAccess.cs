using ApiUsageExamples.Performance.DbModels;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using System;
using System.Diagnostics;

namespace ApiUsageExamples.Performance
{
    public class StorageAccess
    {
        public IMongoDatabase DB { get; set; }
        public IMongoDatabase AdminDB { get; set; }

        public MongoClientSettings MongoClientSettings { get; set; }

        public async Task RunTest()
        {
            Init();

            // Arrange
            await DB.CreateCollectionAsync("Persons", new CreateCollectionOptions<Person>()
            {
                ClusteredIndex = new ClusteredIndexOptions<Person>()
                {
                    Key = "{_id: 1}",
                    Name = "ClusteredIndex",
                    Unique = true
                }
            });

            List<Person> dataToInsert = new List<Person>();
            int i = 0;
            while (i < 100000)
            {
                i++;
                Person p = new Person();
                p.Id = ObjectId.GenerateNewId().ToString();
                p.FirstName = $"FirstName_{i}";
                p.LastName = $"LastName_{i}";
                dataToInsert.Add(p);
            }

            var collection = DB.GetCollection<Person>("Persons");
            await collection.InsertManyAsync(dataToInsert);
        }

        private void Init()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            var connString = "mongodb://localhost:27017";
            var dbName = "PerformanceTests";

            var mongoConnectionUrl = new MongoUrl(connString);
            MongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);

            // http://mongodb.github.io/mongo-csharp-driver/2.8/apidocs/html/N_MongoDB_Driver_Core_Events.htm
            MongoClientSettings.ClusterConfigurator = cb =>
            {
                cb.Subscribe<CommandStartedEvent>(e =>
                {
                    // Console.WriteLine($"{e.CommandName} - {e.Command.ToJson()}");
                });
            };

            // MongoClient must be created after subscribing to the events
            var client = new MongoClient(MongoClientSettings);
            DB = client.GetDatabase(dbName);
            AdminDB = client.GetDatabase("admin");
        }

    }
}
