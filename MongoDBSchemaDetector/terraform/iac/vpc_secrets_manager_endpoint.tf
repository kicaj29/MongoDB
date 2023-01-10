// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/vpc_endpoint
resource "aws_vpc_endpoint" "vpc_secrets_manager_endpoint" {
  vpc_id       = data.aws_vpc.lambda_vpc.id
  service_name = "com.amazonaws.us-east-1.secretsmanager"
  policy = jsonencode({
    "Version" : "2008-10-17",
    "Statement" : [
        {
        "Effect" : "Allow",
        "Principal" : "*",
        "Action" : "*",
        "Resource" : "*"
        }
    ]
    })
  vpc_endpoint_type = "Interface"
  security_group_ids = [
    aws_security_group.lambda_security_group.id,
    data.aws_security_group.lambda_security_group_manually_created.id
  ]
  tags = {
    Name = "InnovationSprintMongoSchemaDetectorSecretsTF"
  }
  depends_on = [
    aws_security_group.lambda_security_group
  ]
}

// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/vpc_endpoint_subnet_association
resource "aws_vpc_endpoint_subnet_association" "vpc_sm_endpoint_subnet1a" {
  vpc_endpoint_id = aws_vpc_endpoint.vpc_secrets_manager_endpoint.id
  subnet_id       = data.aws_subnet.subnet_1a.id
}

resource "aws_vpc_endpoint_subnet_association" "vpc_sm_endpoint_subnet1b" {
  vpc_endpoint_id = aws_vpc_endpoint.vpc_secrets_manager_endpoint.id
  subnet_id       = data.aws_subnet.subnet_1b.id
}

resource "aws_vpc_endpoint_subnet_association" "vpc_sm_endpoint_subnet1c" {
  vpc_endpoint_id = aws_vpc_endpoint.vpc_secrets_manager_endpoint.id
  subnet_id       = data.aws_subnet.subnet_1c.id
}

// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/vpc_endpoint_security_group_association

/*resource "aws_vpc_endpoint_security_group_association" "vpc_sm_endpoint_security_group" {
  vpc_endpoint_id   = aws_vpc_endpoint.vpc_secrets_manager_endpoint.id
  security_group_id = aws_security_group.lambda_security_group.id
}

resource "aws_vpc_endpoint_security_group_association" "vpc_sm_endpoint_security_group_manually_created_lambda" {
  vpc_endpoint_id   = aws_vpc_endpoint.vpc_secrets_manager_endpoint.id
  security_group_id = data.aws_security_group.lambda_security_group_manually_created.id
}*/