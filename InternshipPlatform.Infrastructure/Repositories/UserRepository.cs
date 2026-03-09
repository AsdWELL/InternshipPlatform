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

        public async Task<int> CreateUser(User user)
        {
            context.Entry(user.Role).State = EntityState.Unchanged;

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user.Id;
        }

        public async Task DeleteUserById(int id)
        {
            await context.Users
                .Where(user => user.Id == id)
                .ExecuteDeleteAsync();
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
            await context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.RefreshToken, refreshToken)
                    .SetProperty(u => u.RefreshTokenExpiredAt, refreshTokenExpiredAt));
        }
    }
}
