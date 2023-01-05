using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace InnovationAWSLambdaS3FunctionMongoSchemaDetector
{
    public class RunCommandResultSerializer : SerializerBase<RunCommandResult>
    {
        public override RunCommandResult Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            RunCommandResult result = new RunCommandResult();

            context.Reader.FindElement("n");
            result.n = context.Reader.ReadInt32();

            context.Reader.FindElement("ok");
            result.ok = context.Reader.ReadBoolean();

            return result;
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, RunCommandResult value)
        {
            throw new NotImplementedException();
        }
    }
}
