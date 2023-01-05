using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace InnovationAWSLambdaS3FunctionMongoSchemaDetector
{
    public class MongoConnectionStringProvider : IMongoConnectionStringProvider
    {
        public async Task<string> GetConnectionStringAsync()
        {
            AmazonSecretsManagerConfig secretsManagerConfig = new AmazonSecretsManagerConfig()
            {
                // TODO: remove hardcoded value
                RegionEndpoint = RegionEndpoint.GetBySystemName("us-east-1")
            };

            AmazonSecretsManagerClient client = new AmazonSecretsManagerClient(secretsManagerConfig);
            GetSecretValueResponse secretResponse = await client.GetSecretValueAsync(new GetSecretValueRequest()
            {
                SecretId = "/hxc_platform/storage/username",
                VersionStage = "AWSCURRENT", // VersionStage defaults to AWSCURRENT if unspecified
            });

            return secretResponse.SecretString;
        }
    }
}
