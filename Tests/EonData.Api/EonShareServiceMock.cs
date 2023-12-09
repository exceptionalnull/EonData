using EonData.FileShare.Models;
using EonData.FileShare.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.EonData.Api
{
    internal class EonShareServiceMock : IEonShareService
    {
        private readonly IEnumerable<ShareFolderModel> data;
        public EonShareServiceMock(IEnumerable<ShareFolderModel> mockData) => data = mockData;

        public Task<IEnumerable<ShareFolderModel>> GetFileShareAsync(CancellationToken cancellationToken) => Task.FromResult(data);

        public Task<string> GetSignedUrlAsync(string file, CancellationToken cancellationToken)
        {

            if (data.SelectMany(d => d.Files).Any(f => string.Concat(f.Prefix, f.Name).Equals(file)))
            {
                return Task.FromResult($"https://s3bucket.aws.fake.url/{file}?sid=123ABCD");
            }
            else
            {
                return Task.FromResult(string.Empty);
            }
        }

        public static ShareFileModel CreateShareFileMock(string filename, string prefix) => new() { Name = filename, Prefix = prefix, LastModified = DateTime.UtcNow.AddMinutes(GetRandomNumber(5000) * -1), Size = GetRandomNumber(1000, 3000000) };

        public static ShareFolderModel CreateShareFolderMock(string name, string prefix, int fileCount)
        {
            List<string> randFiles = new();
            while (randFiles.Count() < fileCount)
            {
                randFiles.Add(GetRandomString(GetRandomNumber(8, 16)));
            }
            return CreateShareFolderMock(name, prefix, randFiles);
        }

        public static ShareFolderModel CreateShareFolderMock(string name, string prefix, IEnumerable<string> filenames) => CreateShareFolderMock(name, prefix, filenames.Select(n => CreateShareFileMock(n, prefix)));

        public static ShareFolderModel CreateShareFolderMock(string name, string prefix, IEnumerable<ShareFileModel> files)
        {
            return new ShareFolderModel()
            {
                Name = name,
                Prefix = prefix,
                Files = files
            };
        }

        private static int GetRandomNumber(int maxSize)
        {
            var random = new Random();
            return random.Next(maxSize);
        }
        private static int GetRandomNumber(int minSize, int maxSize)
        {
            var random = new Random();
            return random.Next(minSize, maxSize);
        }

        private static string GetRandomString(int length)
        {
            var random = new Random();
            string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            string result = "";

            for (int i = 0; i < length; i++)
            {
                result += characters[random.Next(characters.Length)];
            }

            return result;
        }

        public Task<bool> FileExistsAsync(string file, CancellationToken cancellationToken) => Task.FromResult(data.SelectMany(d => d.Files).Any(f => f.Key.Equals(file)));
    }
}
