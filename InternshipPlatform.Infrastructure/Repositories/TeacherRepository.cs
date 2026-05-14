using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class TeacherRepository(InternshipPlatformContext context) : ITeacherRepository
    {
        public Task<bool> IsTeacherExists(int id)
        {
            return context.Teachers
                .AsNoTracking()
                .AnyAsync(c => c.UserId == id);
        }

        public async Task AddTeacher(Teacher curator)
        {
            await context.Teachers.AddAsync(curator);
        }

        public Task<Teacher?> GetTeacherById(int id)
        {
            return context.Teachers
                .AsNoTracking()
                .Include(c => c.User)
                .Include(c => c.University)
                .FirstOrDefaultAsync(c => c.UserId == id);
        }

        public Task<Teacher?> GetTeacherForUpdate(int id)
        {
            return context.Teachers
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId == id);
        }

        public async Task UpdateAvatar(int userId, string? avatarPath)
        {
            var curator = await context.Teachers.FindAsync(userId);

            if (curator == null)
                return;

            curator.AvatarPath = avatarPath;
        }
    }
}
