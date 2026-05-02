using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface ICuratorRepository
    {
        Task<bool> IsCuratorExists(int id);
        
        Task AddCurator(Curator curator);

        Task<Curator?> GetCuratorById(int id);

        Task<Curator?> GetCuratorForUpdate(int id);

        Task UpdateAvatar(int userId, string? avatarPath);
    }
}
