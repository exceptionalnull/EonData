# EonData
The eondata.net web application.

The web app consists of a number of parts. The angular front-end is intended to be hosted in S3 and served from a CloudFront distribution. The ASP.NET Core Web API should be hosted on an EC2 instance and served from another CloudFront distribution. The SSL certs for the CloudFront distributions must be in a specific region so they are in a separate CloudFormation stack and shouold be deployed first. The AWS infrastructure should mostly build itself. The last peice is the Azure AD B2C auth system which needs to be configured manually. (TODO: Writeup B2C instructions...)
