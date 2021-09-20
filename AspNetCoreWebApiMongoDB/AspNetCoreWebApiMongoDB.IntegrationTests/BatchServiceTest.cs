using AspNetCoreWebApiMongoDB.Services;
using NUnit.Framework;

namespace AspNetCoreWebApiMongoDB.IntegrationTests
{
    public class BatchServiceTest
    {
        private MongoConnectionString _mongoConnectionString;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var connString = TestContext.Parameters.Get("mongoConnectionString");
            var databaseName = TestContext.Parameters.Get("databaseName");
            this._mongoConnectionString = new MongoConnectionString()
            {
                ConnectionString = connString,
                DatabaseName = databaseName
            };


        }

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestMultipleUpdates()
        {
            var batchService = new BatchService(this._mongoConnectionString);
        }
    }
}