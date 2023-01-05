using Amazon.Lambda.Core;
using MongoDB.Driver;

namespace InnovationAWSLambdaS3FunctionMongoSchemaDetector
{
    public class QueriesExecutor
    {
        private readonly IMongoConnectionStringProvider _connStringProvider;
        private readonly ILambdaLogger _logger;

        public QueriesExecutor(IMongoConnectionStringProvider connStringProvider, ILambdaLogger logger)
        {
            _connStringProvider = connStringProvider;
            _logger = logger;
        }

        public async Task RunAsync(QueryList queries)
        {
            string connectiongString = await _connStringProvider.GetConnectionStringAsync();
            _logger.LogInformation($"Retrieved connection string: {connectiongString}");

            /*MongoClient client = new MongoClient(connectiongString);

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
            }*/
        }
    }
}
