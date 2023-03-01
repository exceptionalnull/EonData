#!/bin/bash
echo "Uploading angular webapp to S3 bucket..."
aws s3 sync ~/projects/EonData/EonData.Web/dist/eon-data.web/ s3://eondata-web-test/