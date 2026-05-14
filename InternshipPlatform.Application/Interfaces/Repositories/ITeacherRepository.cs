using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface ITeacherRepository
    {
        Task<bool> IsTeacherExists(int id);
        
        Task AddTeacher(Teacher curator);

        Task<Teacher?> GetTeacherById(int id);

        Task<Teacher?> GetTeacherForUpdate(int id);

        Task UpdateAvatar(int userId, string? avatarPath);
    }
}
