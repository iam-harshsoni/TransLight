using Microsoft.AspNetCore.Http;
using sds.Service.IServices;

namespace sds.Service
{
    public class LocalStorageService(
        IWebHostEnvironment _env,
        IHttpContextAccessor _httpContextAccessor
        ) : IStorageService
    {

        private static readonly HashSet<string> _allowedMimeType = new(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg", "image/png", "image/gif", "image/webp",
            "application/pdf",
            "application/msword",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        };

        private const long MaxFileSizeBytes = 10 * 1024 * 1024; //10MB
        public async Task<string> SaveAsync(IFormFile file, string folder, CancellationToken ct = default)
        {
            if (file is null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            if (file.Length > MaxFileSizeBytes)
                throw new InvalidOperationException($"File exceeds the maximum size of {MaxFileSizeBytes / (10 * 1024 * 1024)} MB.");

            if (!_allowedMimeType.Contains(file.ContentType))
                throw new InvalidOperationException($"File type {file.ContentType} is not allowed.");

            //Build physical path: /wwwroot/uploads/{folder}
            var uploadRoot = Path.Combine(_env.WebRootPath, "uploads", folder);
            Directory.CreateDirectory(uploadRoot);

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(uploadRoot, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream, ct);

            //return Path.Combine("uploads", folder, fileName).Replace('\\', '/');
            return fileName;
        }

        public Task DeleteAsync(string path, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(path))
                return Task.CompletedTask;

            var fullPath = Path.Combine(_env.WebRootPath, path.TrimStart('/'));
            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }

        public string GetUrl(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            var ctx = _httpContextAccessor.HttpContext;
            if (ctx is null)
                return $"/{path.TrimStart('/')}";

            var request = ctx.Request;
            return $"{request.Scheme}://{request.Host}/{path.TrimStart('/')}";
        }

    }
}
