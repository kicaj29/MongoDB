resource "aws_s3_bucket" "bucket" {
  bucket = var.s3_bucket_name
}

// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/s3_bucket_acl
// terraform destroy does not delete the S3 Bucket ACL but does remove the resource from Terraform state.
resource "aws_s3_bucket_acl" "bucket-acl" {
  bucket = aws_s3_bucket.bucket.id
  acl    = "private"
}

// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/s3_bucket_public_access_block
resource "aws_s3_bucket_public_access_block" "bucket-no-public-access" {
  bucket = aws_s3_bucket.bucket.id

  block_public_acls       = true
  block_public_policy     = true
  ignore_public_acls      = true
  restrict_public_buckets = true
}

// TODO: there was an error for the first run when "The specified bucket does not exist" maybe use depends on phrase
// Error uploading object to S3 bucket (innovation-sprint-mongodb-schema-detector-tf): NoSuchBucket: The specified bucket does not exist   
// status code: 404, request id: KXSDCNM5R2XYV223, host id: DEymz7J2jIQXhhRH91kuZbV3uVLTkMc1Vb10m/jJ2fsGRR6BRCzfIZbBGMNQ5N8+qwqkbyDO8pM= 

// https://ilhicas.com/2019/08/17/Creating-a-folder-s3-terraform.html
resource "aws_s3_bucket_object" "bucket-input" {
  depends_on = [aws_s3_bucket.bucket] // TODO: did not test for the single run that creates everything
  bucket = var.s3_bucket_name
  key    = "input/"
  content_type = "application/x-directory"
}

resource "aws_s3_bucket_object" "bucket-output" {
  depends_on = [aws_s3_bucket.bucket] // TODO: did not test for the single run that creates everything    
  bucket = var.s3_bucket_name
  key    = "output/"
  content_type = "application/x-directory"
}
