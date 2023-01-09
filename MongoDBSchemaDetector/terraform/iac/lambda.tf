resource "aws_cloudwatch_log_group" "lambda_cloudwatch_group" {
  name              = "/aws/lambda/${var.lambda_name}"
  retention_in_days = 14
}

resource "aws_lambda_function" "schema_detector_lambda" {
  function_name = var.lambda_name
  role          = aws_iam_role.lambda_role.arn
  handler       = "InnovationAWSLambdaS3FunctionMongoSchemaDetector::InnovationAWSLambdaS3FunctionMongoSchemaDetector.Function::FunctionHandler"
  runtime       = "dotnet6"
  filename      = "../../InnovationAWSLambdaS3FunctionMongoSchemaDetector/InnovationAWSLambdaS3FunctionMongoSchemaDetector/bin/Release/net6.0/InnovationAWSLambdaS3FunctionMongoSchemaDetector.zip"
  source_code_hash = filebase64sha256("../../InnovationAWSLambdaS3FunctionMongoSchemaDetector/InnovationAWSLambdaS3FunctionMongoSchemaDetector/bin/Release/net6.0/InnovationAWSLambdaS3FunctionMongoSchemaDetector.zip")

  depends_on = [
    aws_cloudwatch_log_group.lambda_cloudwatch_group,
  ]
}


resource "aws_lambda_permission" "allow_trigger_by_s3_bucket" {
  statement_id  = "AllowExecutionFromS3Bucket"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.schema_detector_lambda.arn
  principal     = "s3.amazonaws.com"
  source_arn    = aws_s3_bucket.bucket.arn
}

resource "aws_s3_bucket_notification" "schema_detector_lambda_bucket_notification" {
  bucket = aws_s3_bucket.bucket.id

  lambda_function {
    lambda_function_arn = aws_lambda_function.schema_detector_lambda.arn
    events              = ["s3:ObjectCreated:*"]
    filter_prefix       = "input"
    filter_suffix       = ".json"
  }

  depends_on = [aws_lambda_permission.allow_trigger_by_s3_bucket]
}