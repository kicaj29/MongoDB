using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Diagnostics;

namespace ApiUsageExamples.Tests
{
    public class BaseTest
    {
        public IMongoDatabase DB { get; set; }
        public IMongoDatabase AdminDB { get; set; }

        public MongoClientSettings MongoClientSettings { get; set; }

        private TextWriter _originalConsoleOut;
        private TextWriter _originalConsoleError;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Redirect console output to NUnit's test output
            _originalConsoleOut = Console.Out;
            _originalConsoleError = Console.Error;
            Console.SetOut(new NUnitTextWriter(_originalConsoleOut));
            Console.SetError(new NUnitTextWriter(_originalConsoleError));

            Trace.Listeners.Add(new ConsoleTraceListener());



            var connString = TestContext.Parameters.Get("mongoConnectionString");
            var dbName = TestContext.Parameters.Get("databaseName");

            var mongoConnectionUrl = new MongoUrl(connString);
            MongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);

            var loggerFactory = LoggerFactory.Create(b =>
            {
                b.SetMinimumLevel(LogLevel.Trace);
                b.AddSimpleConsole();
                b.AddSystemdConsole();
                b.AddConsole();
            });

            // Note: to see logs from mongo driver in Visual Studio open view Output -> Tests
            // I was not able to redirect the logs to NUnit's test output in Test Explorer
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

        [OneTimeTearDown]
        public void EndTest()
        {
            Trace.Flush();

            // Restore original console output
            Console.SetOut(_originalConsoleOut);
            Console.SetError(_originalConsoleError);
        }

        [SetUp]
        public void Setup()
        {
        }
    }
}