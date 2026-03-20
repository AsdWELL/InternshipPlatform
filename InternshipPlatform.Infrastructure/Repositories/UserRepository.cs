using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class UserRepository(InternshipPlatformContext context) : IUserRepository
    {
        public async Task<bool> IsUserExists(int userId)
        {
            return await context.Users
                .AnyAsync(user => user.Id == userId);
        }

        public async Task AddUser(User user)
        {
            context.Entry(user.Role).State = EntityState.Unchanged;

            await context.Users.AddAsync(user);
        }

        public async Task DeleteUserById(int id)
        {
            var user = await context.Users
                .Where(user => user.Id == id)
                .FirstOrDefaultAsync();

            if (user == null)
                return;

            context.Users.Remove(user);
        }

        public async Task<Role> GetRoleByName(string roleName)
        {
            return await context.Roles
                .AsNoTracking()
                .FirstAsync(role => role.Name == roleName);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await context.Users
                .AsNoTracking()
                .Include(u => u.Role)
                .FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<User?> GetUserByRefreshToken(string refreshToken)
        {
            return await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.RefreshToken == refreshToken);
        }

        public async Task UpdateRefreshToken(int userId, string? refreshToken, DateTime refreshTokenExpiredAt)
        {
            var user = await context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
                return;

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiredAt = refreshTokenExpiredAt;
        }
    }
}
