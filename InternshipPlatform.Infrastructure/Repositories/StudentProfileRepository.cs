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

        public async Task<StudentProfile?> GetStudentForUpdate(int userId)
        {
            return await context.StudentProfiles
                .Include(sp => sp.User)
                .FirstOrDefaultAsync(st => st.UserId == userId);
        }

        public async Task UpdateAvatar(int userId, string? avatarPath)
        {
            var student = await context.StudentProfiles.FindAsync(userId);

            if (student == null)
                return;

            student.AvatarPath = avatarPath;
        }
    }
}
