using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IStudentProfileRepository
    {
        Task<int> CreateStudentProfile(StudentProfile studentProfile);

        Task<StudentProfile?> GetStudentById(int userId);

        Task<StudentProfile?> GetStudentByEmail(string email);

        Task UpdateStudentProfile(StudentProfile studentProfile);
    }
}
