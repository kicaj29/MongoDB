using MongoDB.Driver.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApiUsageExamples.Tests
{
    internal class UpdateWithPositionalOperatorDollarAndFirstMatchingElementTest : BaseTest
    {

        /// <summary>
        /// https://www.mongodb.com/docs/manual/reference/operator/update/positional/#mongodb-update-up.-
        /// 1. the positional $ operator acts as a placeholder for the first element that matches the query document,
        /// and
        /// 2. the array field must appear as part of the query document
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task UpdateWithPositionalOperator()
        {
            // Arrange
            var collection = DB.GetCollection<DocumentRoot>("Documents");
            DocumentRoot document = new DocumentRoot();
            document.ID = ObjectId.GenerateNewId().ToString();
            document.Actions = new ActionStatus[]
            {
                 new() { Status = "Status_0", StatusProgressCounter = 111 },
                 new() { Status = "Status_1", StatusProgressCounter = 222 },
                 new() { Status = "Status_2", StatusProgressCounter = 333 },
            };
            await collection.InsertOneAsync(document);

            // Act
            FilterDefinition<DocumentRoot> docFilter = Builders<DocumentRoot>.Filter.Eq(d => d.ID, document.ID) &
                Builders<DocumentRoot>.Filter.Eq("Actions.Status", "Status_1");

            UpdateDefinition<DocumentRoot> documentUpdate = Builders<DocumentRoot>.Update
                .Set("Actions.$.StatusProgressCounter", 999);
            await collection.UpdateOneAsync(docFilter, documentUpdate);


            // Assert
            DocumentRoot actualDocument = await collection.Find(Builders<DocumentRoot>.Filter.Eq(d => d.ID, document.ID)).Limit(1).FirstOrDefaultAsync();
            Assert.IsNotNull(actualDocument);
            Assert.IsNotNull(actualDocument.Actions);
            Assert.That(actualDocument.Actions.Length, Is.EqualTo(3));

            // Only second action should be updated
            Assert.That(actualDocument.Actions[0].StatusProgressCounter, Is.EqualTo(111));
            Assert.That(actualDocument.Actions[1].StatusProgressCounter, Is.EqualTo(999));
            Assert.That(actualDocument.Actions[2].StatusProgressCounter, Is.EqualTo(333));

        }

        /// <summary>
        /// Similar scenario like in test <see cref="UpdateWithPositionalOperator"/> but here we replace the whole item in an array.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task UpdateWithFirstMatchingElement()
        {
            // Arrange
            var collection = DB.GetCollection<DocumentRoot>("Documents");
            DocumentRoot document = new DocumentRoot();
            document.ID = ObjectId.GenerateNewId().ToString();
            document.Actions = new ActionStatus[]
            {
                 new() { Status = "Status_0", StatusProgressCounter = 111 },
                 new() { Status = "Status_1", StatusProgressCounter = 222 },
                 new() { Status = "Status_2", StatusProgressCounter = 333 },
            };
            await collection.InsertOneAsync(document);

            // Act
            FilterDefinition<DocumentRoot> docFilter = Builders<DocumentRoot>.Filter.Eq(d => d.ID, document.ID) &
                Builders<DocumentRoot>.Filter.ElemMatch(d => d.Actions, action => action.Status == "Status_1");

            UpdateDefinition<DocumentRoot> documentUpdate = Builders<DocumentRoot>.Update
                .Set(d => d.Actions.FirstMatchingElement(), new ActionStatus()
                {
                    Status = "aaa",
                    StatusProgressCounter = 999
                });
            await collection.UpdateOneAsync(docFilter, documentUpdate);


            // Assert
            DocumentRoot actualDocument = await collection.Find(Builders<DocumentRoot>.Filter.Eq(d => d.ID, document.ID)).Limit(1).FirstOrDefaultAsync();
            Assert.IsNotNull(actualDocument);
            Assert.IsNotNull(actualDocument.Actions);
            Assert.That(actualDocument.Actions.Length, Is.EqualTo(3));

            // Only second action should be updated
            Assert.That(actualDocument.Actions[0].StatusProgressCounter, Is.EqualTo(111));
            Assert.That(actualDocument.Actions[1].Status, Is.EqualTo("aaa"));
            Assert.That(actualDocument.Actions[1].StatusProgressCounter, Is.EqualTo(999));
            Assert.That(actualDocument.Actions[2].StatusProgressCounter, Is.EqualTo(333));

        }
    }
}
