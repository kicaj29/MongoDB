using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.Lambda.TestUtilities;
using Amazon.S3;
using Amazon.S3.Model;
using Moq;
using Xunit;

namespace InnovationAWSLambdaS3FunctionMongoSchemaDetector.Tests;

public class FunctionTest
{
    [Fact]
    public async Task TestS3EventLambdaFunction()
    {
        var mockS3Client = new Mock<IAmazonS3>();
        var getObjectMetadataResponse = new GetObjectMetadataResponse();
        getObjectMetadataResponse.Headers.ContentType = "text/plain";

        mockS3Client
            .Setup(x => x.GetObjectMetadataAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(getObjectMetadataResponse));

        mockS3Client
            .Setup(x => x.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new GetObjectResponse()
            {
                ResponseStream = new FileStream("queries.json", FileMode.Open)
            }));

        var mockMongoConnectionStringProvider = new Mock<IMongoConnectionStringProvider>();
        mockMongoConnectionStringProvider
            .Setup(x => x.GetConnectionStringAsync())
            .Returns(Task.FromResult("mongodb://localhost:27017"));

        // Setup the S3 event object that S3 notifications would create with the fields used by the Lambda function.
        var s3Event = new S3Event
        {
            Records = new List<S3Event.S3EventNotificationRecord>
            {
                new S3Event.S3EventNotificationRecord
                {
                    S3 = new S3Event.S3Entity
                    {
                        Bucket = new S3Event.S3BucketEntity {Name = "s3-bucket" },
                        Object = new S3Event.S3ObjectEntity {Key = "text.txt" }
                    }
                }
            }
        };

        // Invoke the lambda function and confirm the content type was returned.
        ILambdaLogger testLambdaLogger = new TestLambdaLogger();
        var testLambdaContext = new TestLambdaContext
        {
            Logger = testLambdaLogger
        };

        var function = new Function(mockS3Client.Object, mockMongoConnectionStringProvider.Object);
        await function.FunctionHandler(s3Event, testLambdaContext);

        // Assert.Equal("text/plain", ((TestLambdaLogger)testLambdaLogger).Buffer.ToString().Trim());
    }
}