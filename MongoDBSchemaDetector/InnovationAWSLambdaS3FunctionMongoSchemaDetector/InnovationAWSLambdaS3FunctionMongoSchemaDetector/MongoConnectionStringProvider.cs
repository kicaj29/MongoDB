using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System.Text.Json;

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
            GetSecretValueResponse userSecretResponse = await client.GetSecretValueAsync(new GetSecretValueRequest()
            {
                SecretId = "/hxc_platform/storage/username",
                VersionStage = "AWSCURRENT", // VersionStage defaults to AWSCURRENT if unspecified
            });

            GetSecretValueResponse passwordSecretResponse = await client.GetSecretValueAsync(new GetSecretValueRequest()
            {
                SecretId = "/hxc_platform/storage/password",
                VersionStage = "AWSCURRENT", // VersionStage defaults to AWSCURRENT if unspecified
            });

            JsonElement jsonElementKeyValueUser = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(userSecretResponse.SecretString, new JsonSerializerOptions()
            {
                MaxDepth = 1
            });
            string userName = jsonElementKeyValueUser.GetProperty("username").ToString();

            JsonElement jsonElementKeyValuePassword = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(passwordSecretResponse.SecretString, new JsonSerializerOptions()
            {
                MaxDepth = 1
            });
            string password = jsonElementKeyValuePassword.GetProperty("password").ToString();

            return $"mongodb+srv://{userName}:{password}@cluster-sandbox-pl-0.c4b9l.mongodb.net/test";
        }
    }
}
