using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class StudentProfileRepository(InternshipPlatformContext context) : IStudentProfileRepository
    {
        public async Task<bool> IsStudentExists(int userId)
        {
            return await context.StudentProfiles
                .AnyAsync(st => st.UserId == userId);
        }

        public async Task<int> CreateStudentProfile(StudentProfile studentProfile)
        {
            context.StudentProfiles.Add(studentProfile);
            await context.SaveChangesAsync();

            return studentProfile.UserId;
        }

        public async Task<StudentProfile?> GetStudentByEmail(string email)
        {
            var user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return null;

            return await context.StudentProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.UserId == user.Id);
        }

        public async Task<StudentProfile?> GetStudentById(int userId)
        {
            return await context.StudentProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.UserId == userId);
        }

        public async Task UpdateStudentProfile(StudentProfile studentProfile)
        {
            var student = await context.StudentProfiles.FindAsync(studentProfile.UserId);

            if (studentProfile.Name != null)
                student.Name = studentProfile.Name;

            if (studentProfile.Surname != null)
                student.Surname = studentProfile.Surname;

            if (studentProfile.Patronymic != null)
                student.Patronymic = studentProfile.Patronymic;

            if (studentProfile.BirthdayDate != null)
                student.BirthdayDate = studentProfile.BirthdayDate;

            if (studentProfile.Phone != null)
                student.Phone = studentProfile.Phone;

            if (studentProfile.VkLink != null)
                student.VkLink = studentProfile.VkLink;

            if (studentProfile.TgLink != null)
                student.TgLink = studentProfile.TgLink;

            if (studentProfile.GithubLink != null)
                student.GithubLink = studentProfile.GithubLink;

            if (studentProfile.University != null)
                student.University = studentProfile.University;

            if (studentProfile.Specialization != null)
                student.Specialization = studentProfile.Specialization;

            if (studentProfile.GraduationYear != null)
                student.GraduationYear = studentProfile.GraduationYear;

            await context.SaveChangesAsync();
        }
    }
}
