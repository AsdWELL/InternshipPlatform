namespace InternshipPlatform.Domain.Entities
{
    public class Curator
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public int UniversityId { get; set; }

        public University University { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string? Patronymic { get; set; }

        public string? Phone { get; set; }

        public string? VkLink { get; set; }

        public string? TgLink { get; set; }

        public string? MaxLink { get; set; }
    }
}
