namespace InnovationAWSLambdaS3FunctionMongoSchemaDetector
{
    public class QueryList
    {
        public List<QueryDefinition> Queries { get; set; }

        public QueryList()
        {
            Queries = new List<QueryDefinition>();
        }
    }
}
