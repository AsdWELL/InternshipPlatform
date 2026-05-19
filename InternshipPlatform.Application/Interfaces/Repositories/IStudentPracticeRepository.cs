using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IStudentPracticeRepository
    {
        Task AddStudentPractice(StudentPractice studentPractice);
    }
}
