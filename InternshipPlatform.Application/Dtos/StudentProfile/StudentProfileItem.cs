namespace InternshipPlatform.Application.Dtos.StudentProfile
{
    public class StudentProfileItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string? Patronymic { get; set; }

        public string? AvatarPath { get; set; }
    }
}
