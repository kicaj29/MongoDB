namespace InnovationAWSLambdaS3FunctionMongoSchemaDetector
{
    public class RunCommandResult
    {
        public int n { get; set; }
        public bool ok { get; set; }

        public override string ToString()
        {
            return $"n: {n}, ok: {ok}";
        }
    }

}
