using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using System.Text;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace InnovationAWSLambdaS3FunctionMongoSchemaDetector;

public class Function
{
    IAmazonS3 S3Client { get; set; }
    IMongoConnectionStringProvider ConnectionStringProvider { get; set; }

    /// <summary>
    /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
    /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
    /// region the Lambda function is executed in.
    /// </summary>
    public Function()
    {
        S3Client = new AmazonS3Client();
        ConnectionStringProvider = new MongoConnectionStringProvider();
    }

    /// <summary>
    /// Constructs an instance with a preconfigured S3 client. This can be used for testing outside of the Lambda environment.
    /// </summary>
    /// <param name="s3Client"></param>
    /// <param name="connectionStringProvider"></param>
    public Function(IAmazonS3 s3Client, IMongoConnectionStringProvider connectionStringProvider)
    {
        this.S3Client = s3Client;
        this.ConnectionStringProvider = connectionStringProvider;
    }

    /// <summary>
    /// This method is called for every Lambda invocation. This method takes in an S3 event object and can be used
    /// to respond to S3 notifications.
    /// </summary>
    /// <param name="evnt"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task FunctionHandler(S3Event evnt, ILambdaContext context)
    {
        context.Logger.LogInformation("Started processing lambda...");
        var eventRecords = evnt.Records ?? new List<S3Event.S3EventNotificationRecord>();
        foreach (var record in eventRecords)
        {
            var s3Event = record.S3;
            if (s3Event == null)
            {
                continue;
            }

            try
            {
                context.Logger.LogInformation("Starting processing S3 event.");
                var file = await this.S3Client.GetObjectAsync(s3Event.Bucket.Name, s3Event.Object.Key);
                using var reader = new StreamReader(file.ResponseStream);
                var json = await reader.ReadToEndAsync();
                QueryList? queries = JsonSerializer.Deserialize<QueryList>(json);
                Report report = await new QueriesExecutor(ConnectionStringProvider, context.Logger).RunAsync(queries!);

                // https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/character-encoding
                // https://stackoverflow.com/questions/58003293/dotnet-core-system-text-json-unescape-unicode-string
                string reportJson = JsonSerializer.Serialize(report, new JsonSerializerOptions()
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                });
                Byte[] reportBytes = UTF8Encoding.UTF8.GetBytes(reportJson);
                using (MemoryStream reportStream = new MemoryStream(reportBytes))
                {
                    // TODO: create unique name of the file
                    await this.S3Client.UploadObjectFromStreamAsync(s3Event.Bucket.Name, $"output/report.json", reportStream, new Dictionary<string, object>());
                }
                context.Logger.LogInformation("Finished processing S3 event.");
            }
            catch (Exception e)
            {
                context.Logger.LogError($"Error getting object {s3Event.Object.Key} from bucket {s3Event.Bucket.Name}. Make sure they exist and your bucket is in the same region as this function.");
                context.Logger.LogError(e.Message);
                context.Logger.LogError(e.StackTrace);
                throw;
            }
        }
    }
}