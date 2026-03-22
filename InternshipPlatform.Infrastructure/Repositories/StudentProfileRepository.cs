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

        public async Task AddStudentProfile(StudentProfile studentProfile)
        {
            await context.StudentProfiles.AddAsync(studentProfile);
        }

        public async Task<StudentProfile?> GetStudentByEmail(string email)
        {
            return await context.StudentProfiles
                .AsNoTracking()
                .Include(sp => sp.User)
                .FirstOrDefaultAsync(sp => sp.User.Email == email);
        }

        public async Task<StudentProfile?> GetStudentById(int userId)
        {
            return await context.StudentProfiles
                .AsNoTracking()
                .Include(sp => sp.User)
                .FirstOrDefaultAsync(st => st.UserId == userId);
        }

        public async Task UpdateStudentProfile(StudentProfile studentProfile)
        {
            var student = await context.StudentProfiles
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == studentProfile.UserId);

            if (student is null)
                return;

            if (studentProfile.User.Email is not null)
                student.User.Email = studentProfile.User.Email;

            if (studentProfile.User.PasswordHash is not null)
                student.User.PasswordHash = studentProfile.User.PasswordHash;

            if (studentProfile.Name is not null)
                student.Name = studentProfile.Name;

            if (studentProfile.Surname is not null)
                student.Surname = studentProfile.Surname;

            if (studentProfile.Patronymic is not null)
                student.Patronymic = studentProfile.Patronymic;

            if (studentProfile.BirthdayDate is not null)
                student.BirthdayDate = studentProfile.BirthdayDate;

            if (studentProfile.Phone is not null)
                student.Phone = studentProfile.Phone;

            if (studentProfile.VkLink is not null)
                student.VkLink = studentProfile.VkLink;

            if (studentProfile.TgLink is not null)
                student.TgLink = studentProfile.TgLink;

            if (studentProfile.GithubLink is not null)
                student.GithubLink = studentProfile.GithubLink;

            if (studentProfile.University is not null)
                student.University = studentProfile.University;

            if (studentProfile.Specialization is not null)
                student.Specialization = studentProfile.Specialization;

            if (studentProfile.GraduationYear is not null)
                student.GraduationYear = studentProfile.GraduationYear;
        }

        public async Task UpdateAvatar(int userId, string avatarPath)
        {
            var student = await context.StudentProfiles.FindAsync(userId);

            if (student == null)
                return;

            student.AvatarPath = avatarPath;
        }
    }
}
