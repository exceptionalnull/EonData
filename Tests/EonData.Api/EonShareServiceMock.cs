using EonData.FileShare.Models;
using EonData.FileShare.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Tests.EonData.Api
{
    internal class EonShareServiceMock : IEonShareService
    {
        public Task<IEnumerable<ShareFolderModel>> GetFileShareAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSignedUrlAsync(string file, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
