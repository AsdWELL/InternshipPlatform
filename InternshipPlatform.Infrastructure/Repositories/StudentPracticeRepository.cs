using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class StudentPracticeRepository(InternshipPlatformContext context) : IStudentPracticeRepository
    {
        public async Task AddStudentPractice(StudentPractice studentPractice)
        {
            await context.StudentPractices.AddAsync(studentPractice);
        }
    }
}
