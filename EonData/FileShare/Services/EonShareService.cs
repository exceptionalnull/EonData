﻿using Amazon.S3;
using Amazon.S3.Model;

using EonData.FileShare.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonData.FileShare.Services
{
    public class EonShareService : IEonShareService
    {
        private const string EONSHARE_S3_BUCKET = "eonshare";
        private readonly IAmazonS3 s3Client;

        public EonShareService(IAmazonS3 s3) => s3Client = s3;

        public async Task<IEnumerable<ShareFolderModel>> GetFileShareAsync(CancellationToken cancellationToken)
        {
            // list the S3Objects in the share bucket
            var req = new ListObjectsV2Request()
            {
                BucketName = EONSHARE_S3_BUCKET
            };
            var resp = await s3Client.ListObjectsV2Async(req, cancellationToken);

            // convert S3Objects to ShareFileModels
            var files = resp.S3Objects.Select(GetFileModel);

            // process the ShareFileModels into the full fileshare data structure
            var folders = files.GroupBy(f => f.Prefix).Select(g => new ShareFolderModel()
            {
                Name = g.Key.Substring(g.Key.LastIndexOf('/') + 1),
                Prefix = g.Key,
                // when Name == empty it is the directory details...
                Files = g.Where(g => g.Name != string.Empty).ToList(),
                LastModified = g.Where(g => g.Name == string.Empty).FirstOrDefault()?.LastModified
            });

            return folders;
        }

        public async Task<string> GetSignedUrlAsync(string file, CancellationToken cancellationToken)
        {
            var req = new GetPreSignedUrlRequest()
            {
                BucketName = EONSHARE_S3_BUCKET,
                Key = file,
                Expires = DateTime.UtcNow.AddDays(2)
            };

            string shareUrl = string.Empty;
            if (!cancellationToken.IsCancellationRequested)
            {
                shareUrl = await s3Client.GetPreSignedURLAsync(req);
            }
            return shareUrl;
        }

        private ShareFileModel GetFileModel(S3Object fileObject)
        {
            int lastSep = fileObject.Key.LastIndexOf('/');
            return new ShareFileModel()
            {
                Key = fileObject.Key,
                Name = fileObject.Key.Substring(lastSep + 1),
                Prefix = (lastSep > 0) ? fileObject.Key.Substring(0, lastSep) : string.Empty,
                Size = fileObject.Size,
                LastModified = fileObject.LastModified
            };
        }
    }
}
