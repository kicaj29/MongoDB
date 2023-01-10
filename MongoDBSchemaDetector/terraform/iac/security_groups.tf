// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/security_group

resource "aws_security_group" "lambda_security_group" {
  name        = "InnovationSprintSecurityGroupMongoSchemaDetectorTF"
  description = "Security group for lambda schema detector"
  vpc_id      = data.aws_vpc.lambda_vpc.id

  ingress {
    description      = "ingress"
    from_port        = 0
    to_port          = 65535
    protocol         = "tcp"
    cidr_blocks      = ["0.0.0.0/0"]
    ipv6_cidr_blocks = ["::/0"]
  }

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