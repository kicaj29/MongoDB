using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiUsageExamples.Tests
{
    internal class ChangePrimaryKey : BaseTest
    {
        [Test]
        public async Task ChangePK()
        {
            // Arrange
            var collection = DB.GetCollection<Person>("Persons");
            Person person = new Person();
            person.FirstName = "jacek";
            person.LastName = "kowalski";
            await collection.InsertOneAsync(person);
            await collection.DeleteManyAsync(Builders<Person>.Filter.Empty);

            // Act
            var collectionNewPK = DB.GetCollection<PersonNewPK>("Persons");
            PersonNewPK personNewPK = new PersonNewPK();
            // Now FirstName will be PK
            personNewPK.Id = "jacek";
            personNewPK.LastName = "kowalski";
            await collectionNewPK.InsertOneAsync(personNewPK);


            // Assert
            PersonNewPK actualDocument = await collectionNewPK.Find(Builders<PersonNewPK>.Filter.Eq(d => d.FirstName, personNewPK.FirstName)).Limit(1).FirstOrDefaultAsync();
            Assert.IsNotNull(actualDocument);

            // Cleanup
            await collection.DeleteManyAsync(Builders<Person>.Filter.Empty);
        }
    }
}
