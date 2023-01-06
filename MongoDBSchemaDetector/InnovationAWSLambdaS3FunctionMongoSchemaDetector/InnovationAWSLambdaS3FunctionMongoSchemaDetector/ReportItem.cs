namespace InnovationAWSLambdaS3FunctionMongoSchemaDetector
{
    public class ReportItem
    {
        public bool Exists { get; set; }
        public string CollectionName { get; set; }
        public string Query { get; set; }
        public string FriendlyName { get; set; }

        public ReportItem(bool exists, string collectionName, string query, string friendlyName)
        {
            Exists = exists;
            CollectionName = collectionName;
            Query = query;
            FriendlyName = friendlyName;
        }
    }
}
