// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/security_group

resource "aws_security_group" "lambda_security_group" {
  name        = "InnovationSprintSecurityGroupMongoSchemaDetectorTF"
  description = "Security group for lambda schema detector"
  vpc_id      = data.aws_vpc.lambda_vpc.id

  // because this lambda is only triggered by S3 we prohibit any incomming traffic via ENI, this can be done because S3 does not use ENI
  // no ingress means there are no rules and no rules means that all traffic via ENI is blocked 
  // https://github.com/hashicorp/terraform-provider-aws/issues/4399
  ingress = []

  // egress has to set, without it the lambda is not triggered by the S3 (tested it)
  egress {
    from_port        = 0
    to_port          = 65535
    protocol         = "tcp"
    cidr_blocks      = ["0.0.0.0/0"]
    ipv6_cidr_blocks = ["::/0"]
  }

  tags = {
    Name = "InnovationSprintSecurityGroupMongoSchemaDetectorTF"
  }
}