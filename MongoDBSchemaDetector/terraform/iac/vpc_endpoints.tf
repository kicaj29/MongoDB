data "aws_vpc" "lambda_vpc" {
    tags = {
        Name = "hxc"
    }
}

data "aws_route_table" "private_route_table" {
    tags = {
        Name = "hxc-private"
    }
}

// https://medium.com/tensult/creating-vpc-endpoint-for-amazon-s3-using-terraform-7a15c840d36f
// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/vpc_endpoint
resource "aws_vpc_endpoint" "vpc_s3_endpoint" {
  vpc_id       = data.aws_vpc.lambda_vpc.id
  service_name = "com.amazonaws.us-east-1.s3"
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
  tags = {
    Name = "InnovationSprintMongoSchemaDetectorS3TF"
  }
}

// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/vpc_endpoint_route_table_association
resource "aws_vpc_endpoint_route_table_association" "vpc_s3_endpoint_route_table_association" {
  route_table_id  = data.aws_route_table.private_route_table.id
  vpc_endpoint_id = aws_vpc_endpoint.vpc_s3_endpoint.id
}

/*resource "aws_vpc_endpoint_policy" "vpc_s3_endpoint_policy" {
  vpc_endpoint_id = aws_vpc_endpoint.vpc_s3_endpoint.id
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
}*/

// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/vpc_endpoint_subnet_association
// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/vpc_endpoint_security_group_association