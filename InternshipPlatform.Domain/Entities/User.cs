namespace InternshipPlatform.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public bool IsVerified { get; set; }

        public DateTime LastLogin { get; set; }

        public Role Role { get; set; }
    }
}
