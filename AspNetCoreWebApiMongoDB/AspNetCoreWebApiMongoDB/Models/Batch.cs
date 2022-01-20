using AspNetCoreWebApiMongoDB.Serializers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApiMongoDB.Models
{
    public class Batch
    {
        public string _id { get; set; }
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        //[BsonSerializer(typeof(BatchStateV1Serializer))]
        public BatchStateV1 State { get; set; }

        //public BatchState State { get; set; }

        public BatchSuspension Suspension { get; set; }

        public Concurrency Concurrency { get; set; }

        public List<string> DocumentIDs { get; set; }

    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BatchState
    {
        Unknown = 0,
        Created = 1,
        ReadyToProcess = 2,
        Processing = 3,
        ReadyForVerification = 4,
        Verification = 5,
        ReadyForExport = 6,
        Exported = 100,
        Complete = 101
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BatchStateV1
    {
        Unknown = 0,
        Created = 1,
        ReadyToProcess = 2,
        Processing = 3,
        Complete = 101,
        Deleted = 9999
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BatchSuspension
    {
        None = 0,
        Suspending = 1,
        Suspended = 2
    }

    public class Concurrency
    {
        public string UserName { get; set; }
    }

}
