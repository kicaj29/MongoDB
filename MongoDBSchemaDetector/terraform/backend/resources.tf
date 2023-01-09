
// dynamodb table
resource "aws_dynamodb_table" "terraform_statelock" {
  name           = "innovation-sprint-terraform-state-table"
  read_capacity  = 20
  write_capacity = 20
  hash_key       = "LockID"     #It is required to use this value to have working terraform backend with s3 and dynamodb

  attribute {
    name = "LockID"             #It is required to use this value to have working terraform backend with s3 and dynamodb       
    type = "S"                  #It is required to use this value to have working terraform backend with s3 and dynamodb (S means string)
  }
}

//s3

resource "aws_s3_bucket" "bucket" {
  bucket = "innovation-sprint-terraform-state"
}

// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/s3_bucket_acl
// terraform destroy does not delete the S3 Bucket ACL but does remove the resource from Terraform state.
resource "aws_s3_bucket_acl" "bucket-acl" {
  bucket = aws_s3_bucket.bucket.id
  acl    = "private"
}

resource "aws_s3_bucket_versioning" "bucket-with-versioning" {
  bucket        = aws_s3_bucket.bucket.id
  versioning_configuration {
    status = "Enabled"
  }
}

// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/s3_bucket_public_access_block
resource "aws_s3_bucket_public_access_block" "bucket-no-public-access" {
  bucket = aws_s3_bucket.bucket.id

  block_public_acls       = true
  block_public_policy     = true
  ignore_public_acls      = true
  restrict_public_buckets = true
}