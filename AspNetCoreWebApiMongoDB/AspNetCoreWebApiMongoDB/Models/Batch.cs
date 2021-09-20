﻿using MongoDB.Bson;
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
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        public BatchState State { get; set; }

        public BatchSuspension Suspension { get; set; }

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
    public enum BatchSuspension
    {
        None = 0,
        Suspending = 1,
        Suspended = 2
    }
}
