namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IImageService
    {
        Task<string> SaveCompanyLogo(Stream stream, string extension);

        Task<string> SaveStudentProfileAvatar(Stream stream, string extension);

        Task<string> SaveCuratorProfileAvatar(Stream stream, string extension);

        Task DeleteIfExists(string? relativePath);
    }
}
