using Amazon.S3;

using EonData.FileShare.Services;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.EonData
{
    internal class FileShareTests
    {
        [Fact]
        private async Task GetS3Result()
        {
            var mockS3 = new Mock<IAmazonS3>();

            var service = new EonShareService(mockS3.Object);
        }
    }
}
