using MongoDB.Driver;
using System.Dynamic;

namespace MongoDBSchemaDetector
{
    public class RulesManager
    {
        public async Task<IEnumerable<DetectorResultItem>> RunAsync()
        {
            MongoUrl mongoUrl = new MongoUrl("mongodb://localhost:27017");
            MongoClientSettings clientSettings = MongoClientSettings.FromUrl(mongoUrl);
            MongoClient client = new MongoClient(clientSettings);

            List<DetectorResultItem> results = new List<DetectorResultItem>();

            using (IAsyncCursor<string> cursor = await client.ListDatabaseNamesAsync())
            {
                await cursor.ForEachAsync(dbName =>
                {
                    if (dbName == "testDB")
                    {
                        IMongoDatabase db = client.GetDatabase(dbName);
                        //TODO: use RunCommandAsync
                        // Use empty projection to make sure that we do not return any values
                        //dynamic expandoResult = db.RunCommand(new JsonCommand<dynamic>("{\"find\": \"collection1\", \"filter\": {\"ID\": \"b32a1626-65be-4cc1-81a8-0b6e5cea28d0\" }, \"projection\": {\"_id\": 0, \"BatchState\": 0} }"));
                        dynamic expandoResult = db.RunCommand(new JsonCommand<dynamic>("{\"count\": \"collection1\", \"query\": {\"ID\": \"b32a1626-65be-4cc1-81a8-0b6e5cea28d0\" }, \"limit\": 1 }"));
                        results.Add(new DetectorResultItem()
                        {
                            RuleName = "Rule1",
                            Exists = expandoResult.n >= 1
                        });
                    }
                });
            }

            return results;


        }
    }
}
