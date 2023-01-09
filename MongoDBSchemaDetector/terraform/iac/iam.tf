// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/iam_role

data "aws_iam_policy" "policy-ready-storage-secrets" {
  // arn worked only in the account id was in the arn
  name = "hxc-read-storage-secrets"
}

resource "aws_iam_role" "lambda_role" {
  name = "InnovationSprintMongoSchemaDetectorRoleTF"

  # Terraform's "jsonencode" function converts a
  # Terraform expression result to valid JSON syntax.
  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Principal = {
          Service = "lambda.amazonaws.com"
        }
      },
    ]
  })

  inline_policy {
    name = "InnovationS3ReadWriteAccessLambdaMongoSchematDetector"

    policy = jsonencode({
      Version = "2012-10-17"
      Statement = [
        {
          Action   = ["s3:PutObject", "s3:GetObject"]
          Effect   = "Allow"
          Resource = "arn:aws:s3:::innovation-sprint-mongodb-schema-detector-tf/*"
        },        
      ]
    })
  }
}

resource "aws_iam_role_policy_attachment" "lambda-read-storage-secrets" {
  role       = aws_iam_role.lambda_role.id
  policy_arn = data.aws_iam_policy.policy-ready-storage-secrets.arn
}

resource "aws_iam_role_policy_attachment" "lambda-basic-execution" {
  role       = aws_iam_role.lambda_role.id
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
}

resource "aws_iam_role_policy_attachment" "lambda-vpc-create-network-interfaces" {
  role       = aws_iam_role.lambda_role.id
  policy_arn = "arn:aws:iam::aws:policy/AmazonVPCCrossAccountNetworkInterfaceOperations"
}