using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiFindOneAndUpdateAsync
{
    public class Worker
    {
        public async Task StartProcessing()
        {
            MongoConnectionString connString = new MongoConnectionString()
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "TestDB"
            };

            var mongoConnectionUrl = new MongoUrl(connString.ConnectionString);
            var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);
            var client = new MongoClient(mongoClientSettings);

            var db = client.GetDatabase(connString.DatabaseName);
            IMongoCollection<MongoPerson> persons = db.GetCollection<MongoPerson>("persons");

            await persons.DeleteManyAsync(Builders<MongoPerson>.Filter.Empty);
            var personId = MongoDB.Bson.ObjectId.GenerateNewId();

            await persons.InsertOneAsync(new MongoPerson()
            {
                Id = personId,
                FirstName = "Jacek",
                LastName = "Placek"
            });

            int tasksCount = 200;
            Task[] firstNameUpdates = new Task[tasksCount];
            for(int index = 0; index < tasksCount; index++)
            {
                firstNameUpdates[index] = Update(index);
            }

            // It looks that returned object always has a state that reflects made changes, it means
            // that changes done by other tasks are not visible for the selected task
            async Task Update(int index)
            {
                var newFirstName = Guid.NewGuid().ToString();
                var newLastName = Guid.NewGuid().ToString();
                var after = await persons.FindOneAndUpdateAsync(Builders<MongoPerson>.Filter.Eq(p => p.Id, personId),
                    Builders<MongoPerson>.Update.Set(p => p.FirstName, newFirstName)
                        .Set(p => p.LastName, newLastName),
                    new FindOneAndUpdateOptions<MongoPerson, MongoPerson>
                    {
                        ReturnDocument = ReturnDocument.After
                    });
                if (after.FirstName != newFirstName)
                {
                    throw new Exception("FirstName has different value then expected");
                }
                else
                {
                    Console.WriteLine($"{index}: correct new first name {newFirstName}");
                }
                if (after.LastName != newLastName)
                {
                    throw new Exception("LastName has different value then expected");
                }
                else
                {
                    Console.WriteLine($"{index}: correct new last name {newLastName}");
                }
            }

            await Task.WhenAll(firstNameUpdates);


        }



    }
}
