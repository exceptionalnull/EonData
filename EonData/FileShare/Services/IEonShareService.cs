using EonData.FileShare.Models;

namespace EonData.FileShare.Services
{
    public interface IEonShareService
    {
        Task<IEnumerable<ShareFolderModel>> GetFileShareAsync(CancellationToken cancellationToken);
        Task<string> GetSignedUrlAsync(string file, CancellationToken cancellationToken);
    }
}