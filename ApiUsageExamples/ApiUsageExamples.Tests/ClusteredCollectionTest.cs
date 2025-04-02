using System.Dynamic;

namespace ApiUsageExamples.Tests
{
    internal class ClusteredCollectionTest : BaseTest
    {
        [Test]
        public async Task CreateClusteredCollection()
        {
            // Arrange
            await DB.CreateCollectionAsync("Persons", new CreateCollectionOptions<Person>()
            {
                ClusteredIndex = new ClusteredIndexOptions<Person>()
                {
                    Key = "{_id: 1}",
                    Name = "ClusteredIndex",
                    Unique = true
                }
            });

            List<Person> dataToInsert = new List<Person>();
            int i = 0;
            while(i < 10)
            {
                i++;
                Person p = new Person();
                p.Id = ObjectId.GenerateNewId().ToString();
                p.FirstName = $"FirstName_{i}";
                p.LastName = $"LastName_{i}";
                dataToInsert.Add(p);
            }

            var collection = DB.GetCollection<Person>("Persons");
            await collection.InsertManyAsync(dataToInsert);
            ExpandoObject runCommandResult = DB.RunCommand(new JsonCommand<ExpandoObject>("{ listCollections: 1 }"));

            string actualOptions = ((ExpandoObject)((List<Object>)((ExpandoObject)runCommandResult.ElementAt(0).Value).ElementAt(2).Value)[0]).ElementAt(2).ToJson();
            string expectedOptions = "{ \"k\" : \"options\", \"v\" : { \"clusteredIndex\" : { \"v\" : 2, \"key\" : { \"_id\" : 1 }, \"name\" : \"ClusteredIndex\", \"unique\" : true } } }";

            // https://www.mongodb.com/docs/manual/core/clustered-collections/#determine-if-a-collection-is-clustered
            Assert.That(actualOptions, Is.EqualTo(expectedOptions));
        }
    }
}
