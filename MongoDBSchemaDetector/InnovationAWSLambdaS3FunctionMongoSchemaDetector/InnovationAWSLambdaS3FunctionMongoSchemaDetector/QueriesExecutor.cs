using MongoDB.Driver;

namespace InnovationAWSLambdaS3FunctionMongoSchemaDetector
{
    public class QueriesExecutor
    {
        private readonly IMongoConnectionStringProvider _connStringProvider;
        public QueriesExecutor(IMongoConnectionStringProvider connStringProvider)
        {
            _connStringProvider = connStringProvider;
        }

        public async Task RunAsync(QueryList queries)
        {
            MongoClient client = new MongoClient(await _connStringProvider.GetConnectionStringAsync());

            using (IAsyncCursor<string> cursor = await client.ListDatabaseNamesAsync())
            {
                await cursor.ForEachAsync(dbName =>
                {
                    if (dbName == "testDB")
                    {
                        IMongoDatabase db = client.GetDatabase(dbName);
                        //TODO: use RunCommandAsync
                        foreach (QueryDefinition query in queries.Queries)
                        {
                            if (!query.Ignore)
                            {
                                dynamic expandoResult = db.RunCommand(new JsonCommand<dynamic>($"{{\"count\": \"{query.CollectionName}\", \"query\": {query.Query}, \"limit\": 1 }}"));
                                bool exists = expandoResult.n >= 1;
                            }
                        }
                    }
                });
            }
        }
    }
}
