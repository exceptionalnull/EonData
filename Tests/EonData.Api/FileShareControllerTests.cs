using EonData.Api.Controllers;
using EonData.FileShare.Models;
using EonData.FileShare.Services;

using Microsoft.AspNetCore.Mvc;

namespace Tests.EonData.Api
{
    public class FileShareControllerTests
    {
        [Fact]
        public async Task GetFileShareDetailsReturnsResult()
        {
            List<ShareFolderModel> data = new()
            {
                EonShareServiceMock.CreateShareFolderMock("", "", 10),
                EonShareServiceMock.CreateShareFolderMock("folder", "folder/", 6),
                EonShareServiceMock.CreateShareFolderMock("test", "folder/test", 5)
            };
            IEonShareService service = new EonShareServiceMock(data);
            var controller = new FileShareController(service);

            ActionResult<IEnumerable<ShareFolderModel>> response = await controller.GetFileShareDetails(new CancellationToken());

            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equal(data, okResult.Value);
        }

        [Fact]
        public async Task ValidDownloadRedirects()
        {
            List<ShareFolderModel> data = new()
            {
                EonShareServiceMock.CreateShareFolderMock("", "", new List<string>(){ "example.txt", "image.png", "archive.zip" }),
                EonShareServiceMock.CreateShareFolderMock("folder", "folder/", new List<string>(){ "nested.txt", "subfolder.png" }),
                EonShareServiceMock.CreateShareFolderMock("test", "folder/test", new List<string>(){ "example.txt", "image.png", "archive.zip" }),
                EonShareServiceMock.CreateShareFolderMock("asdf", "asdf/", new List<string>(){ "qwerty.txt", "zxcvb.png" }),
            };
            IEonShareService service = new EonShareServiceMock(data);
            var controller = new FileShareController(service);

            IActionResult response = await controller.DownloadFile("example.txt", new CancellationToken());

            var redirect = Assert.IsType<RedirectResult>(response);
            Assert.Equal("https://s3bucket.aws.fake.url/example.txt?sid=123ABCD", redirect.Url);
        }

        [Fact]
        public async Task InvalidDownload404Redirects()
        {
            List<ShareFolderModel> data = new()
            {
                EonShareServiceMock.CreateShareFolderMock("", "", new List<string>(){ "example.txt", "image.png", "archive.zip" }),
                EonShareServiceMock.CreateShareFolderMock("folder", "folder/", new List<string>(){ "nested.txt", "subfolder.png" }),
                EonShareServiceMock.CreateShareFolderMock("test", "folder/test", new List<string>(){ "example.txt", "image.png", "archive.zip" }),
                EonShareServiceMock.CreateShareFolderMock("asdf", "asdf/", new List<string>(){ "qwerty.txt", "zxcvb.png" }),
            };
            IEonShareService service = new EonShareServiceMock(data);
            var controller = new FileShareController(service);

            IActionResult response = await controller.DownloadFile("not-here.txt", new CancellationToken());

            var redirect = Assert.IsType<NotFoundResult>(response);
        }
    }
}