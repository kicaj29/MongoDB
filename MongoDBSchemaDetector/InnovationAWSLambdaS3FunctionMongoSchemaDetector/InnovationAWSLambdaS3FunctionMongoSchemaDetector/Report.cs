namespace InnovationAWSLambdaS3FunctionMongoSchemaDetector
{
    public class Report
    {
        public List<ReportItem> Items { get; set; } = new List<ReportItem>();

        public Report()
        {

        }

        public Report(List<ReportItem> items)
        {
            Items = items;
        }
    }
}
