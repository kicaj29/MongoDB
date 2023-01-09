// https://medium.com/tensult/creating-vpc-endpoint-for-amazon-s3-using-terraform-7a15c840d36f

// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/vpc_endpoint
resource "aws_vpc_endpoint" "vpc_s3_endpoint" {
  vpc_id       = aws_vpc.main.id
  service_name = "com.amazonaws.us-east-1.s3"
}

// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/vpc_endpoint_route_table_association

resource "aws_vpc_endpoint_route_table_association" "vpc_s3_endpoint_route_table_association" {
  route_table_id  = aws_route_table.example.id
  vpc_endpoint_id = aws_vpc_endpoint.vpc_s3_endpoint.id
}

// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/vpc_endpoint_subnet_association

// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/vpc_endpoint_security_group_association