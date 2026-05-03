using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using System.Security.Cryptography;
using System.Text;

namespace InternshipPlatform.Infrastructure.Services
{
    public class InviteCodeGenerator(IStudentGroupRepository studentGroupRepository) : IInviteCodeGenerator
    {
        private const string Alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        public async Task<string> GenerateAsync()
        {
            string inviteCode;

            do
            {
                var rawCode = GenerateRawCode(8);

                inviteCode = $"{rawCode[..4]}-{rawCode[4..]}";
            }
            while (await studentGroupRepository.IsInviteCodeExists(inviteCode));

            return inviteCode;
        }

        private static string GenerateRawCode(int length)
        {
            var result = new StringBuilder(length);

            using var rng = RandomNumberGenerator.Create();

            var bytes = new byte[length];
            rng.GetBytes(bytes);

            foreach (var b in bytes)
                result.Append(Alphabet[b % Alphabet.Length]);

            return result.ToString();
        }
    }
}
