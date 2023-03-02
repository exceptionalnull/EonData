#!/bin/bash
echo "Uploading angular webapp to S3 bucket..."
aws s3 sync ~/projects/EonData/EonData.Web/dist/eon-data.web/ s3://eondataweb/
echo "Uploading web api to S3 bucket..."
aws s3 sync ~/projects/EonData/EonData.Api/bin/Release/net7.0/ s3://eondata/bin/eondata-api/