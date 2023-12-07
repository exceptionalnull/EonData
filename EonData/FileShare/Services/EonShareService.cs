using Amazon.S3;
using Amazon.S3.Model;

using EonData.FileShare.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonData.FileShare.Services
{
    public class EonShareService
    {
        private const string EONSHARE_S3_BUCKET = "eonshare";
        private readonly IAmazonS3 s3Client;

        public EonShareService(IAmazonS3 s3) => s3Client = s3;

        public async Task<IEnumerable<ShareFolderModel>> GetFileShareAsync(CancellationToken cancellationToken)
        {
            var req = new ListObjectsV2Request()
            {
                BucketName = EONSHARE_S3_BUCKET
            };
            var resp = await s3Client.ListObjectsV2Async(req, cancellationToken);
            var files = resp.S3Objects.Select(o => new ShareFileModel
            {
                Name = o.Key,
                Size = o.Size
            });
            var folders = files.GroupBy(f => GetPrefix(f.Name)).Select(g => new ShareFolderModel
            {
                Prefix = g.Key,
                Files = g.ToList()
            });

            return folders;
        }

        public async Task<string> GetSignedUrlAsync(string file, CancellationToken cancellationToken) {
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

        private string GetPrefix(string key)
        {
            var lastIndex = key.LastIndexOf('/');
            return lastIndex >= 0 ? key.Substring(0, lastIndex) : string.Empty;
        }
    }
}
