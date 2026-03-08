using BCrypt.Net;
using InternshipPlatform.Application.Interfaces.Services.Auth;

namespace InternshipPlatform.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly HashType _hashType = HashType.SHA256;

        public string Generate(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, _hashType);
        }

        public bool Verify(string password, string hashPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashPassword, _hashType);
        }
    }
}