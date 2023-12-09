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

namespace Tests.EonData.FileShare
{
    public class FileShareTests
    {
        private const string S3OBJECT_JSON = @"FileShare\defaultFileObjects.json";
        private const string FILESHARE_JSON = @"FileShare\defaultFileShare.json";

        [Fact]
        public async Task GetsFileShareDetails()
        {
            var mockS3 = new Mock<IAmazonS3>();
            var mockFileObjects = GetDefaultFileObjects();
            var expectedFileShare = GetDefaultFileShare();
            mockS3.Setup(s3 => s3.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ListObjectsV2Response() { S3Objects = mockFileObjects });

            var service = new EonShareService(mockS3.Object);
            var result = await service.GetFileShareAsync(new CancellationToken());

            Assert.Equivalent(expectedFileShare, result);
        }

        [Fact]
        public async Task FileShareDetailsNeverHasFileWithNoName()
        {
            var mockS3 = new Mock<IAmazonS3>();
            mockS3.Setup(s3 => s3.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ListObjectsV2Response() { S3Objects = GetDefaultFileObjects() });

            var service = new EonShareService(mockS3.Object);
            var result = await service.GetFileShareAsync(new CancellationToken());

            // the files with empty names are information about folders and should not be included in the final data structure.
            // they are used to determine lastModified for directories but are hidden from the final output.
            var isNoNameFile = result.SelectMany(d => d.Files).Any(f => string.IsNullOrEmpty(f.Name));
            Assert.False(isNoNameFile);
        }

        [Fact]
        public async Task CanCreateSignedUrl()
        {
            var mockS3 = new Mock<IAmazonS3>();
            const string testFilename = "P1000895.JPG";
            mockS3.Setup(s3 => s3.GetPreSignedURLAsync(It.IsAny<GetPreSignedUrlRequest>()))
                .Callback<GetPreSignedUrlRequest>(req => { Assert.Equal(testFilename, req.Key); })
                .ReturnsAsync("https://test.com");
            var service = new EonShareService(mockS3.Object);
            var result = await service.GetSignedUrlAsync(testFilename, new CancellationToken());
        }

        [Fact]
        public async Task DetectsWhenFileDoesExist()
        {
            var mockS3 = new Mock<IAmazonS3>();
            mockS3.Setup(s3 => s3.GetObjectMetadataAsync(It.IsAny<GetObjectMetadataRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new GetObjectMetadataResponse());
            var service = new EonShareService(mockS3.Object);
            var result = await service.FileExistsAsync("example.txt", new CancellationToken());
            Assert.True(result);
        }

        [Fact]
        public async Task DetectsWhenFileDoesNotExist()
        {
            var mockS3 = new Mock<IAmazonS3>();
            mockS3.Setup(s3 => s3.GetObjectMetadataAsync(It.IsAny<GetObjectMetadataRequest>(), It.IsAny<CancellationToken>())).ThrowsAsync(new AmazonS3Exception("") { StatusCode = System.Net.HttpStatusCode.NotFound });
            var service = new EonShareService(mockS3.Object);
            var result = await service.FileExistsAsync("example.txt", new CancellationToken());
            Assert.False(result);
        }

        private struct StoredS3ObjDetails
        {
            public string key { get; set; }
            public DateTime lastModified { get; set; }
            public long size { get; set; }
        }

        private static List<S3Object> GetDefaultFileObjects() => JsonSerializer.Deserialize<IEnumerable<StoredS3ObjDetails>>(File.ReadAllText(S3OBJECT_JSON))
                ?.Select(x => new S3Object()
                {
                    BucketName = "eontest-bucket",
                    Key = x.key,
                    LastModified = x.lastModified,
                    Size = x.size
                }).ToList() ?? new();

        private static IEnumerable<ShareFolderModel> GetDefaultFileShare() => JsonSerializer.Deserialize<IEnumerable<ShareFolderModel>>(File.ReadAllText(FILESHARE_JSON), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? new List<ShareFolderModel>();
    }
}
