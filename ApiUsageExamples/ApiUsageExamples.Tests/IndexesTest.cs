using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiUsageExamples.Tests
{
    internal class IndexesTest : BaseTest
    {
        [Test]
        public async Task ManageIndexes()
        {
            // http://mongodb.github.io/mongo-csharp-driver/2.8/apidocs/html/N_MongoDB_Driver_Core_Events.htm
            // Starting from Mongo 6.0 there is possibility to listen DDL events but not sure how to get this info using Mongo Driver.
            // https://www.mongodb.com/docs/manual/reference/change-events/
            // Next line of code throws error: Command aggregate failed: The $changeStream stage is only supported on replica sets
            // var cursor = await AdminDB.Aggregate().ChangeStream(new ChangeStreamStageOptions() { AllChangesForCluster = true }).ToCursorAsync();
            // Assert.NotNull(cursor);

            IndexKeysDefinition<Person> indexDefinition = Builders<Person>.IndexKeys.Ascending(key => key.LastName);
            // By default indexes are not unique
            CreateIndexModel<Person> indexModel = new(indexDefinition, new CreateIndexOptions() { Unique = true, Name = nameof(Person.LastName) });
            var collection = DB.GetCollection<Person>("Persons");
            // see in the BaseTest events that createIndex command is sent to the mongo server
            await collection.Indexes.CreateOneAsync(indexModel);

            Person p = new Person();
            p.FirstName = "Jacek";
            p.LastName = "Placek";
            await collection.InsertOneAsync(p);

            var personRead = (await collection.Find(Builders<Person>.Filter.Eq(filter => filter.Id, p.Id)).Limit(1).ToListAsync())[0];
            await collection.DeleteOneAsync(Builders<Person>.Filter.Eq(filter => filter.Id, personRead.Id));

            IndexKeysDefinition<Person> indexDefinition2 = Builders<Person>.IndexKeys.Ascending(key => key.FirstName);
            CreateIndexModel<Person> indexModel2 = new(indexDefinition2, new CreateIndexOptions() { Name = nameof(Person.LastName) });
            // There is no way to update index. In such case we have to drop then index and create new one.
            MongoCommandException exception = Assert.ThrowsAsync<MongoCommandException>(async () => await collection.Indexes.CreateOneAsync(indexModel2));
            Assert.That("IndexKeySpecsConflict", Is.EqualTo(exception.CodeName));

        }
    }
}
