using AspNetCoreWebApiMongoDB.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApiMongoDB.Serializers
{
    public class BatchStateV1Serializer : SerializerBase<BatchStateV1>
    {
        public override BatchStateV1 Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var valFromDB = context.Reader.ReadInt32();
            if (valFromDB == 5) // OR other values that we want mapp to Processing
            {
                return BatchStateV1.Processing;
            }
            else
            {
                return base.Deserialize(context, args);
            }
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, BatchStateV1 value)
        {
            base.Serialize(context, args, value);
        }
    }
}
