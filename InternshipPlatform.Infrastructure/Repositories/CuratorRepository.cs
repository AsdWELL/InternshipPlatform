using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class CuratorRepository(InternshipPlatformContext context) : ICuratorRepository
    {
        public Task<bool> IsCuratorExists(int id)
        {
            return context.Curators
                .AsNoTracking()
                .AnyAsync(c => c.UserId == id);
        }

        public async Task AddCurator(Curator curator)
        {
            await context.Curators.AddAsync(curator);
        }

        public Task<Curator?> GetCuratorById(int id)
        {
            return context.Curators
                .AsNoTracking()
                .Include(c => c.User)
                .Include(c => c.University)
                .FirstOrDefaultAsync(c => c.UserId == id);
        }

        public Task<Curator?> GetCuratorForUpdate(int id)
        {
            return context.Curators
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId == id);
        }

        public async Task UpdateAvatar(int userId, string? avatarPath)
        {
            var curator = await context.Curators.FindAsync(userId);

            if (curator == null)
                return;

            curator.AvatarPath = avatarPath;
        }
    }
}
