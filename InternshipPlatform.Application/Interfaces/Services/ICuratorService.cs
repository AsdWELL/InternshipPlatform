using InternshipPlatform.Application.Dtos.Curator;
using Microsoft.AspNetCore.Http;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface ICuratorService
    {
        Task<CuratorResponse> GetCuratorById(int id);

        Task UpdateCuratorProfile(UpdateCuratorRequest request);

        Task UpdateCuratorAvatar(int id, IFormFile avatarFile);

        Task DeleteAvatar(int id);

        Task Logout(int id);

        Task DeleteCurator(int id);
    }
}
