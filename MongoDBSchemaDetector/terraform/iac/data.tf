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

data "aws_iam_policy" "policy-ready-storage-secrets" {
  // arn worked only if the account id was in the arn so I used name
  name = "hxc-read-storage-secrets"
}

// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/data-sources/subnet
data "aws_subnet" "subnet_1a" {
  tags = {
    Name = "hxc-private-us-east-1a"
  }
}

data "aws_subnet" "subnet_1b" {
  tags = {
    Name = "hxc-private-us-east-1b"
  }
}

data "aws_subnet" "subnet_1c" {
  tags = {
    Name = "hxc-private-us-east-1c"
  }
}

// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/data-sources/security_group
// this is temporary to keep working lambda created manually
data "aws_security_group" "lambda_security_group_manually_created" {
  name = "InnovationSprintSecurityGroupMongoSchemaDetector"
}