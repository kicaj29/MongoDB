terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 3"
    }
  }

  backend "s3" {
    bucket = "innovation-sprint-terraform-state"
    key = "mongo-schema-detector"
    region = "us-east-1"
    profile = "sandbox-sso"
  }
}
