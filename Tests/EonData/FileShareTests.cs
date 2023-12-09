using Amazon.S3;
using Amazon.S3.Model;

using EonData.FileShare.Models;
using EonData.FileShare.Services;

using Moq;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tests.EonData
{
    public class FileShareTests
    {
        private const string S3OBJECT_JSON = "defaultFileObjects.json";
        private const string FILESHARE_JSON = "defaultFileShare.json";

        [Fact]
        public async Task ReturnsFileShareDetails()
        {
            var mockS3 = new Mock<IAmazonS3>();
            var mockFileObjects = GetDefaultFileObjects();
            var expectedFileShare = GetDefaultFileShare();
            mockS3.Setup(s3 => s3.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ListObjectsV2Response() { S3Objects = mockFileObjects });

            var service = new EonShareService(mockS3.Object);
            var result = await service.GetFileShareAsync(new CancellationToken());

            Assert.Equivalent(expectedFileShare, result);
        }

        private static List<S3Object> GetDefaultFileObjects() => JsonSerializer.Deserialize<IEnumerable<StoredS3ObjDetails>>(File.ReadAllText(S3OBJECT_JSON))
                ?.Select(x => new S3Object()
                {
                    BucketName = "eontest-bucket",
                    Key = x.key,
                    LastModified = x.lastModified,
                    Size = x.size
                }).ToList() ?? new();

        private struct StoredS3ObjDetails
        {
            public string key { get; set; }
            public DateTime lastModified { get; set; }
            public long size { get; set; }
        }

        private static IEnumerable<ShareFolderModel> GetDefaultFileShare() => JsonSerializer.Deserialize<IEnumerable<ShareFolderModel>>(File.ReadAllText(FILESHARE_JSON), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true  }) ?? new List<ShareFolderModel>();
    }
}
