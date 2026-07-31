using Microsoft.AspNetCore.Http;

namespace sds.Service.IServices
{
    public interface IStorageService
    {
        Task<string> SaveAsync(IFormFile file, string folder, CancellationToken ct = default);
        Task DeleteAsync(string path, CancellationToken ct = default);
        string GetUrl(string Path);
    }
}
