using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<bool> IsUserExists(int userId);
        
        Task<int> CreateUser(User user);

        Task<Role> GetRoleByName(string roleName);

        Task<User?> GetUserByEmail(string email);

        Task<User?> GetUserByRefreshToken(string refreshToken);

        Task UpdateRefreshToken(int userId, string? refreshToken, DateTime refreshTokenExpiredAt);

        Task DeleteUserById(int id);
    }
}
