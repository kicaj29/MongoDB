using Amazon.Lambda.Core;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace InnovationAWSLambdaS3FunctionMongoSchemaDetector
{
    public class QueriesExecutor
    {
        private readonly IMongoConnectionStringProvider _connStringProvider;
        private readonly ILambdaLogger _logger;


        static QueriesExecutor()
        {
            // MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(typeof(RunCommandResult), new RunCommandResultSerializer());
            BsonClassMap.RegisterClassMap<RunCommandResult>(p =>
            {
                p.AutoMap();
                p.SetIgnoreExtraElements(true);
            });
        }

        public QueriesExecutor(IMongoConnectionStringProvider connStringProvider, ILambdaLogger logger)
        {
            _connStringProvider = connStringProvider;
            _logger = logger;
        }

        public async Task RunAsync(QueryList queries)
        {
            _logger.LogInformation($"There are {queries.Queries.Count(q => !q.Ignore)} queries which will be processed.");

            _logger.LogInformation("Retrieving connection string.");
            string connectiongString = await _connStringProvider.GetConnectionStringAsync();

            MongoClient client = new MongoClient(connectiongString);

            _logger.LogInformation("Listing databases.");
            using (IAsyncCursor<string> cursor = await client.ListDatabaseNamesAsync())
            {
                List<string> databases = cursor.ToList();
                _logger.LogInformation($"Retrieved {databases.Count(d => d.StartsWith("CAP_"))} databases names which starts from 'CAP_'");
                for(int index = 0; index < databases.Count; index++)
                {
                    string dbName = databases[index];
                    if (dbName.StartsWith("CAP_"))
                    {
                        _logger.LogInformation($"Processing DB...");
                        IMongoDatabase db = client.GetDatabase(dbName);
                        //TODO: use RunCommandAsync
                        foreach (QueryDefinition query in queries.Queries)
                        {
                            if (!query.Ignore)
                            {
                                // TODO: stop processing query after first finding!
                                _logger.LogInformation($"Executing query {query.Query} for collection {query.CollectionName}.");
                                RunCommandResult runCommandResult = db.RunCommand(new JsonCommand<RunCommandResult>($"{{\"count\": \"{query.CollectionName}\", \"query\": {query.Query}, \"limit\": 1 }}"));
                                bool exists = runCommandResult.n >= 1;
                                _logger.LogInformation($"Query {query.Query} result is: {exists}.");
                            }
                            else
                            {
                                _logger.LogInformation($"Skipping query {query.FriendlyName} because it is marked as ignored.");
                            }
                        }
                    }
                    else
                    {
                        _logger.LogInformation($"Skipping DB because its name does not start from 'CAP_'");
                    }
                }
            }
        }
    }
}
