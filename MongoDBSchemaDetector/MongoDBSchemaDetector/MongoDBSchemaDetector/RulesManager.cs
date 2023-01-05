using MongoDB.Driver;

namespace MongoDBSchemaDetector
{
    public class RulesManager
    {
        public async Task<IEnumerable<DetectorResultItem>> RunAsync()
        {
            MongoUrl mongoUrl = new MongoUrl("mongodb://localhost:27017");
            MongoClientSettings clientSettings = MongoClientSettings.FromUrl(mongoUrl);
            MongoClient client = new MongoClient(clientSettings);

            QueryList queryList = new QueryList();
            queryList.Queries.Add(new QueryDefinition()
            {
                CollectionName = "collection1",
                FriendlyName = "Exist item with ID = b32a1626-65be-4cc1-81a8-0b6e5cea28d0",
                Query = "{\"ID\": \"b32a1626-65be-4cc1-81a8-0b6e5cea28d0\"}"
            });

            List<DetectorResultItem> results = new List<DetectorResultItem>();

            using (IAsyncCursor<string> cursor = await client.ListDatabaseNamesAsync())
            {
                await cursor.ForEachAsync(dbName =>
                {
                    if (dbName == "testDB")
                    {
                        IMongoDatabase db = client.GetDatabase(dbName);
                        //TODO: use RunCommandAsync
                        foreach(QueryDefinition query in queryList.Queries)
                        {
                            dynamic expandoResult = db.RunCommand(new JsonCommand<dynamic>($"{{\"count\": \"{query.CollectionName}\", \"query\": {query.Query}, \"limit\": 1 }}"));
                            results.Add(new DetectorResultItem()
                            {
                                RuleName = query.FriendlyName!,
                                Exists = expandoResult.n >= 1
                            });
                        }
                    }
                });
            }

            return results;


        }
    }
}
