using InternshipPlatform.Application.Dtos.File;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<SavedFileResult> SavePracticeMaterial(Stream stream, string originalFileName, string extension);

        Task<SavedFileResult> SaveStudentReport(Stream stream, string originalFileName, string extension);

        Task<SavedFileResult> SaveStudentSolution(Stream stream, string originalFileName, string extension);

        Task<FileDownloadResult?> GetFile(string relativePath);

        Task DeleteFileIfExists(string? relativePath);
    }
}
