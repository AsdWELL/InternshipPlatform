namespace InternshipPlatform.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiredAt { get; set; }

        public Role Role { get; set; }
    }
}
