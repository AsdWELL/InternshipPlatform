namespace InternshipPlatform.Application.Dtos.PracticePeriod
{
    public class TeacherPracticeGroupStudentItem
    {
        public int StudentId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string? Patronymic { get; set; }

        public string? AvatarPath { get; set; }

        public string? Email { get; set; }
    }
}