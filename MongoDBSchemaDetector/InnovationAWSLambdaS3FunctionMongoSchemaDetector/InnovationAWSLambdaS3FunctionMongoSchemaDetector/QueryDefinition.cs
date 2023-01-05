namespace InnovationAWSLambdaS3FunctionMongoSchemaDetector
{
    public class QueryDefinition
    {
        public string CollectionName { get; set; }
        public string Query { get; set; }
        public string FriendlyName { get; set; }
        public bool Ignore { get; set; }

        public QueryDefinition(string collectionName, string query, string friendlyName, bool ignore)
        {
            CollectionName = collectionName;
            Query = query;
            FriendlyName = friendlyName;
            Ignore = ignore;
        }
    }
}
