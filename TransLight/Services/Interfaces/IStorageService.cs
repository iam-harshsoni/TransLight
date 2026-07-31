namespace TransLight.Services.Interfaces
{
    public interface IStorageService
    {
        Task<string> SaveAsync(IFormFile file, string folder, CancellationToken ct = default);
        //Task DeleteAsync(string path, CancellationToken ct = default);
        Task DeleteAsync(string fileName, string folder, CancellationToken cancellationToken = default);
        string GetUrl(string Path);
    }
}
