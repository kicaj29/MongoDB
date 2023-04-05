using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            // 1.   There is no way to update index. In such case we have to drop the index and create new one.
            // 1A.  Try to update index by changing keys (LastName -> FirstName) and using the same name.
            IndexKeysDefinition<Person> indexDefinition1A = Builders<Person>.IndexKeys.Ascending(key => key.FirstName);
            CreateIndexModel<Person> indexModel1A = new(indexDefinition1A, new CreateIndexOptions() { Unique = true, Name = nameof(Person.LastName) });
            MongoCommandException exception = Assert.ThrowsAsync<MongoCommandException>(async () => await collection.Indexes.CreateOneAsync(indexModel1A));
            Assert.That("IndexKeySpecsConflict", Is.EqualTo(exception.CodeName));
            Debug.WriteLine(exception.Message);
            // Command createIndexes failed: Index must have unique name.The existing index: { v: 2, unique: true, key: { LastName: 1 }, name: "LastName" } has the same name as the requested index: { v: 2, unique: true, key: { FirstName: 1 }, name: "LastName" }.

            // 1B.  Try to change index name (use the same model but just with different name)
            CreateIndexModel<Person> indexModel1B = new(indexDefinition, new CreateIndexOptions() { Unique = true, Name = "LastNameBetterName" });
            exception = Assert.ThrowsAsync<MongoCommandException>(async () => await collection.Indexes.CreateOneAsync(indexModel1B));
            Assert.That("IndexOptionsConflict", Is.EqualTo(exception.CodeName));
            Debug.WriteLine(exception.Message); // Command createIndexes failed: Index with name: LastNameBetterName already exists with a different name.

            // 2. Index name is unique in scope of its collection, different collections can use the same index names
            IndexKeysDefinition<Customer> customerIndexDefinition = Builders<Customer>.IndexKeys.Ascending(key => key.LastName);
            // By default indexes are not unique
            CreateIndexModel<Customer> indexModelCustomer = new(customerIndexDefinition, new CreateIndexOptions() { Unique = true, Name = nameof(Customer.LastName) });
            var collectionCusomters = DB.GetCollection<Customer>("Customers");
            Assert.DoesNotThrowAsync(async () => await collectionCusomters.Indexes.CreateOneAsync(indexModelCustomer));


        }
    }
}
