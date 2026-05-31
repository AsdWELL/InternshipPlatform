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

        public Task<bool> IsStudentHasGroup(int studentId)
        {
            return context.StudentProfiles
                .AsNoTracking()
                .AnyAsync(sp => sp.UserId == studentId && sp.GroupId != null);
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
                .Include(sp => sp.Group)
                    .ThenInclude(g => g.University)
                .Include(sp => sp.Group)
                    .ThenInclude(g => g.EducationalProgram)
                .FirstOrDefaultAsync(sp => sp.User.Email == email);
        }

        public async Task<StudentProfile?> GetStudentById(int userId)
        {
            return await context.StudentProfiles
                .AsNoTracking()
                .Include(sp => sp.User)
                .Include(sp => sp.Group)
                    .ThenInclude(g => g.University)
                .Include(sp => sp.Group)
                    .ThenInclude(g => g.EducationalProgram)
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

        public async Task<string?> GetStudentEmailById(int userId)
        {
            return await context.StudentProfiles
                .AsNoTracking()
                .Where(sp => sp.UserId == userId)
                .Select(sp => sp.User.Email)
                .FirstOrDefaultAsync();
        }
    }
}
