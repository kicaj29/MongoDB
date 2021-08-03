using AspNetCoreWebApiMongoDB.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApiMongoDB.Services
{
    public class CrewService
    {
        private readonly IMongoCollection<Crew> _crews;
        private readonly ProjectionDefinition<Crew> _projection;

        public CrewService(MongoConnectionString connString)
        {
            var client = new MongoClient(connString.ConnectionString);
            var db = client.GetDatabase(connString.DatabaseName);


            // to select only part of the fields we have to define projection,
            // without it we get an exception
            // id by default is included
            this._projection = Builders<Crew>.Projection
                .Include(c => c.CrewName)
                .Include(c => c.Skills);

            this._crews = db.GetCollection<Crew>("crew");
        }

        public List<Crew> GetCrews()
        {
            // just an example how to read only subset of fields:
            var tmpList = this._crews.Find(Builders<Crew>.Filter.Empty).Project(this._projection).ToList();
            foreach(var item in tmpList)
            {
               Debug.WriteLine(item.GetValue("name"));
            }

            return this._crews.Find(Builders<Crew>.Filter.Empty).ToList();
        }
    }
}
