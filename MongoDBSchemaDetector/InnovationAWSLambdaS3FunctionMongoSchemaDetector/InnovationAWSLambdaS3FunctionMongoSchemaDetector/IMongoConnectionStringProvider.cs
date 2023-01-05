namespace InnovationAWSLambdaS3FunctionMongoSchemaDetector
{
    public interface IMongoConnectionStringProvider
    {
        Task<string> GetConnectionStringAsync();
    }
}
