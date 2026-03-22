namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IImageService
    {
        Task<string> SaveCompanyLogo(Stream stream, string extension);

        Task DeleteIfExists(string? relativePath);
    }
}
