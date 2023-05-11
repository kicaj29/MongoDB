using ApiUsageExamples.Performance.DbModels;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using System;
using System.Collections.Generic;
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
            await DB.CreateCollectionAsync("Persons_ClusteredCollection", new CreateCollectionOptions<Person>()
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
            while (i < 10000)
            {
                i++;
                Person p = new Person();
                p.Id = ObjectId.GenerateNewId().ToString();
                p.FirstName = $"FirstName_{i}";
                p.LastName = $"LastName_{i}";
                p.Data1 = new List<string>();
                p.Data2 = new List<string>();
                p.Data3 = new List<string>();
                p.Data4 = new List<string>();

                int x = 0;
                while(x < 0)
                {
                    x++;
                    p.Data1.Add($"Data_{x}");
                    p.Data2.Add($"Data_{x}");
                    p.Data3.Add($"Data_{x}");
                    p.Data4.Add($"Data_{x}");
                }

                dataToInsert.Add(p);
            }

            Random random = new Random();
            List<string> idsRandomOrder = dataToInsert.Select(d => d.Id).OrderBy(d => random.Next().ToString()).ToList();

            var clusteredCollection = DB.GetCollection<Person>("Persons_ClusteredCollection");
            await clusteredCollection.InsertManyAsync(dataToInsert);

            var nonClusteredCollection = DB.GetCollection<Person>("Persons_NonClusteredCollection");
            await nonClusteredCollection.InsertManyAsync(dataToInsert);

            // Act
            Stopwatch sw = Stopwatch.StartNew();
            int queriesAmount = 10000;
            for (int index = 0; index < queriesAmount; index++)
            {
                List<Person> personsFromDB = await clusteredCollection.Find(Builders<Person>.Filter.Eq(p1 => p1.Id, idsRandomOrder[index])).Limit(1).ToListAsync();
            }
            Debug.WriteLine($"Read from clustered collection: {sw.Elapsed}.");
            sw.Restart();

            for (int index = 0; index < queriesAmount; index++)
            {
                List<Person> personsFromDB = await nonClusteredCollection.Find(Builders<Person>.Filter.Eq(p1 => p1.Id, idsRandomOrder[index])).Limit(1).ToListAsync();
            }
            Debug.WriteLine($"Read from non clustered collection: {sw.Elapsed}.");
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
