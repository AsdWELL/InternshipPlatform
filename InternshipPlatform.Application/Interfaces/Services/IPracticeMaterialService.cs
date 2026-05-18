using Microsoft.AspNetCore.Http;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IPracticeMaterialService
    {
        Task UploadMaterials(int employerId, int practiceOfferId, IEnumerable<IFormFile> files);

        Task DeleteFile(int employerId, int practiceOfferId, int fileId);
    }
}
