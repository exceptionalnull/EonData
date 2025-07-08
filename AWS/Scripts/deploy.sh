#!/bin/bash

DEPLOY_WEB=false
DEPLOY_API=false

show_help() {
  echo "Usage: $0 [--web] [--api]"
  echo "  --web   Deploy the Angular web app"
  echo "  --api   Deploy the Web API"
  echo "  --help  Show this help message"
}

# Show help if no arguments
if [ $# -eq 0 ]; then
  show_help
  exit 1
fi

# Parse arguments
for arg in "$@"; do
  case $arg in
    --web)
      DEPLOY_WEB=true
      ;;
    --api)
      DEPLOY_API=true
      ;;
    --help|-h)
      show_help
      exit 0
      ;;
    *)
      echo "Unknown option: $arg"
      show_help
      exit 1
      ;;
  esac
done

# Deploy web app if selected
if [ "$DEPLOY_WEB" = true ]; then
  echo "Uploading Angular webapp to S3 bucket..."
  aws s3 sync ~/projects/EonData/EonData.Web/dist/eon-data.web/browser/ s3://eondataweb/ --delete --exact-timestamps --exclude 3rdpartylicenses.txt
  aws s3 cp ~/projects/EonData/EonData.Web/dist/eon-data.web/3rdpartylicenses.txt s3://eondataweb/
fi

# Deploy API if selected
if [ "$DEPLOY_API" = true ]; then
  echo "Uploading Web API to S3 bucket..."
  aws s3 sync ~/projects/EonData/EonData.Api/bin/Publish/Release/net8.0/linux-x64/ s3://eondataweb-data/bin/api/ --delete --exact-timestamps
fi
