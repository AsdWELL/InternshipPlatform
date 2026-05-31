using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IStudentProfileRepository
    {
        Task<bool> IsStudentExists(int userId);

        Task<bool> IsStudentHasGroup(int studentId);

        Task AddStudentProfile(StudentProfile studentProfile);

        Task<StudentProfile?> GetStudentById(int userId);

        Task<StudentProfile?> GetStudentForUpdate(int userId);

        Task<StudentProfile?> GetStudentByEmail(string email);

        Task UpdateAvatar(int userId, string? avatarPath);

        Task<string?> GetStudentEmailById(int userId);
    }
}
