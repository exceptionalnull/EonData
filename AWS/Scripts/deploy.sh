#!/bin/bash
echo "Uploading angular webapp to S3 bucket..."
aws s3 sync ~/projects/EonData/EonData.Web/dist/eon-data.web/ s3://eondataweb/ --delete --exact-timestamps
echo "Uploading web api to S3 bucket..."
aws s3 sync ~/projects/EonData/EonData.Api/bin/Publish/Release/net7.0/linux-x64/ s3://eondataweb-data/bin/api/ --delete --exact-timestamps