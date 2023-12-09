using EonData.Api.Controllers;
using EonData.FileShare.Models;
using EonData.FileShare.Services;

using Microsoft.AspNetCore.Mvc;

using Moq;

using System.Text.Json;

namespace Tests.EonData.Api
{
    public class FileShareControllerTests
    {
        private const string FILESHARE_JSON = @"defaultFileShare.json";

        [Fact]
        public async Task GetFileShareDetailsReturnsResult()
        {
            var mockData = GetDefaultFileShare();
            var mockService = new Mock<IEonShareService>();
            mockService.Setup(fs => fs.GetFileShareAsync(It.IsAny<CancellationToken>())).ReturnsAsync(mockData);
            var controller = new FileShareController(mockService.Object);
            var response = await controller.GetFileShareDetails(new CancellationToken());

            Assert.IsType<ActionResult<IEnumerable<ShareFolderModel>>>(response);
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equivalent(mockData, okResult.Value);
        }

        [Fact]
        public async Task ValidDownloadRedirects()
        {
            var mockService = new Mock<IEonShareService>();
            mockService.Setup(fs => fs.GetSignedUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Callback<string, CancellationToken>((fkey, _) =>
                {
                    Assert.Equal("example.txt", fkey);
                })
                .ReturnsAsync("https://s3bucket.aws.fake.url/example.txt?sid=123ABCD");
            mockService.Setup(fs => fs.FileExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Callback<string, CancellationToken>((fkey, _) =>
                {
                    Assert.Equal("example.txt", fkey);
                })
                .ReturnsAsync(true);
            var controller = new FileShareController(mockService.Object);
            var response = await controller.DownloadFile("example.txt", new CancellationToken());

            Assert.NotNull(response);
            var redirect = Assert.IsType<RedirectResult>(response);
            Assert.Equal("https://s3bucket.aws.fake.url/example.txt?sid=123ABCD", redirect.Url);
        }

        [Fact]
        public async Task InvalidDownloadReturns404()
        {
            var mockService = new Mock<IEonShareService>();
            mockService.Setup(fs => fs.GetSignedUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Callback<string, CancellationToken>((fkey, _) =>
                {
                    Assert.Fail("This method should not be called when a file does not exist.");
                })
                .ReturnsAsync("https://s3bucket.aws.fake.url/example.txt?sid=123ABCD");
            mockService.Setup(fs => fs.FileExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Callback<string, CancellationToken>((fkey, _) =>
                {
                    Assert.Equal("example.txt", fkey);
                })
                .ReturnsAsync(false);
            var controller = new FileShareController(mockService.Object);
            var response = await controller.DownloadFile("example.txt", new CancellationToken());

            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response);
        }

        private static IEnumerable<ShareFolderModel> GetDefaultFileShare() => JsonSerializer.Deserialize<IEnumerable<ShareFolderModel>>(File.ReadAllText(FILESHARE_JSON), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? new List<ShareFolderModel>();
    }
}