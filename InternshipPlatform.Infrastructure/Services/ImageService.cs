using InternshipPlatform.Application.Exceptions.Company.Logo;
using InternshipPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;

namespace InternshipPlatform.Infrastructure.Services
{
    public class ImageService(IWebHostEnvironment env) : IImageService
    {
        private const string UploadsFolder = "uploads";
        private const string CompanyLogoFolder = "company-logos";

        private static readonly HashSet<string> AllowedExtensions =
        [
            ".jpg", ".jpeg", ".png", ".webp"
        ];

        public async Task<string> SaveCompanyLogo(Stream stream, string extension)
        {
            extension = extension.ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
                throw new InvalidLogoTypeException(AllowedExtensions);

            var folder = Path.Combine(env.WebRootPath, UploadsFolder, CompanyLogoFolder);
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var fullPath = Path.Combine(folder, fileName);

            await using var fstream = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write);
            await stream.CopyToAsync(fstream);

            return $"/{UploadsFolder}/{CompanyLogoFolder}/{fileName}";
        }

        public Task DeleteIfExists(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return Task.CompletedTask;

            var normalized = relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.GetFullPath(Path.Combine(env.WebRootPath, normalized));
            var webRoot = Path.GetFullPath(env.WebRootPath);

            if (!fullPath.StartsWith(webRoot, StringComparison.Ordinal))
                throw new InvalidOperationException("Некорректный путь к файлу.");

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }
    }
}
