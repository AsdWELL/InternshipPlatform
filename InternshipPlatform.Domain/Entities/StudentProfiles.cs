namespace InternshipPlatform.Domain.Entities
{
    public class StudentProfiles
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public DateTime BirthdayDate { get; set; }

        public string Phone { get; set; }

        public string VkLink { get; set; }

        public string TgLink { get; set; }

        public string GithubLink { get; set; }

        public string University { get; set; }

        public string Specialization { get; set; }

        public int GraduationYear { get; set; }
    }
}
