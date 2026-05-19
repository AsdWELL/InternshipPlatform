using InternshipPlatform.Application.Dtos.File;
using InternshipPlatform.Application.Exceptions.File;
using InternshipPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;

namespace InternshipPlatform.Infrastructure.Services
{
    public class FileStorageService(IWebHostEnvironment env) : IFileStorageService
    {
        private const string FilesFolder = "storage";
        
        private string PracticeMaterialsFolder => Path.Combine(FilesFolder, "practices");
        
        private string StudentReportsFolder => Path.Combine(FilesFolder, "students", "reports");

        private string StudentSolutionsFolder => Path.Combine(FilesFolder, "students", "solutions");

        private readonly HashSet<string> AllowedTextFileExtensions =
        [
                ".pdf",
                ".docx",
                ".doc",
                ".xlsx",
                ".xls",
                ".pptx",
                ".txt"
        ];

        private readonly HashSet<string> AllowedArchiveExtensions =
        [
            ".zip",
            ".rar",
            ".7z"
        ];

        private async Task<SavedFileResult> SaveFile(Stream stream,
            string originalFileName,
            string folder,
            string extension)
        {
            var originalSafeFileName = Path.GetFileName(originalFileName);

            var storageRoot = Path.GetFullPath(Path.Combine(env.ContentRootPath, FilesFolder));
            var targetFolder = Path.GetFullPath(Path.Combine(env.ContentRootPath, folder));

            if (!targetFolder.StartsWith(storageRoot, StringComparison.Ordinal))
                throw new InvalidOperationException("Некорректный путь к папке.");

            Directory.CreateDirectory(targetFolder);

            var storedFileName = $"{Guid.NewGuid():N}{extension}";
            var fullPath = Path.Combine(targetFolder, storedFileName);

            await using var fileStream = new FileStream(
                fullPath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None);

            await stream.CopyToAsync(fileStream);

            var relativePath = Path.GetRelativePath(env.ContentRootPath, fullPath)
                .Replace(Path.DirectorySeparatorChar, '/');

            return new SavedFileResult
            {
                OriginalFileName = originalSafeFileName,
                RelativePath = relativePath
            };
        }
        
        public Task<SavedFileResult> SavePracticeMaterial(Stream stream, string originalFileName, string extension)
        {
            extension = extension.ToLowerInvariant();

            HashSet<string> allowedExtensions = [..AllowedTextFileExtensions, ..AllowedArchiveExtensions];

            if (!allowedExtensions.Contains(extension))
                throw new InvalidFileTypeException(allowedExtensions);

            return SaveFile(stream, originalFileName, PracticeMaterialsFolder, extension);
        }

        public Task<SavedFileResult> SaveStudentReport(Stream stream, string originalFileName, string extension)
        {
            extension = extension.ToLowerInvariant();

            if (!AllowedTextFileExtensions.Contains(extension))
                throw new InvalidFileTypeException(AllowedTextFileExtensions);

            return SaveFile(stream, originalFileName, StudentReportsFolder, extension);
        }

        public Task<SavedFileResult> SaveStudentSolution(Stream stream, string originalFileName, string extension)
        {
            extension = extension.ToLowerInvariant();

            if (!AllowedArchiveExtensions.Contains(extension))
                throw new InvalidFileTypeException(AllowedArchiveExtensions);

            return SaveFile(stream, originalFileName, StudentSolutionsFolder, extension);
        }

        public Task<FileDownloadResult?> GetFile(string relativePath)
        {
            var storageRoot = Path.GetFullPath(Path.Combine(env.ContentRootPath, FilesFolder));

            var normalized = relativePath
                .TrimStart('/')
                .Replace('/', Path.DirectorySeparatorChar);

            var fullPath = Path.GetFullPath(Path.Combine(env.ContentRootPath, normalized));

            if (!fullPath.StartsWith(storageRoot, StringComparison.Ordinal))
                throw new InvalidOperationException("Некорректный путь к файлу.");

            if (!File.Exists(fullPath))
                return Task.FromResult<FileDownloadResult?>(null);

            var stream = new FileStream(
                fullPath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read);

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(fullPath, out var contentType))
                contentType = "application/octet-stream";

            return Task.FromResult<FileDownloadResult?>(new FileDownloadResult
            {
                Stream = stream,
                ContentType = contentType
            });
        }

        public Task DeleteFileIfExists(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return Task.CompletedTask;

            var storageRoot = Path.GetFullPath(Path.Combine(env.ContentRootPath, FilesFolder));

            var normalized = relativePath
                .TrimStart('/')
                .Replace('/', Path.DirectorySeparatorChar);

            var fullPath = Path.GetFullPath(Path.Combine(env.ContentRootPath, normalized));

            if (!fullPath.StartsWith(storageRoot, StringComparison.Ordinal))
                throw new InvalidOperationException("Некорректный путь к файлу.");

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }
    }
}
