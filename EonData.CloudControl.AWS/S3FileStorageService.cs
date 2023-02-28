using Amazon.S3;

using System.Text;

namespace EonData.CloudControl.AWS
{
    public class S3FileStorageService
    {
        private readonly IAmazonS3 _amazonS3;
        public S3FileStorageService(IAmazonS3 amazonS3Service)
        {
            _amazonS3 = amazonS3Service;
        }

        public async Task WriteFile(string bucketName, string path, string data, CancellationToken cancellationToken)
        {
            using var dataStream = new MemoryStream(Encoding.Default.GetBytes(data));
            if (dataStream != null)
            {
                await _amazonS3.UploadObjectFromStreamAsync(bucketName, path, dataStream, null, cancellationToken);
            }
        }
    }
}