namespace InternshipPlatform.Application.Dtos.PracticeApplication
{
    public class EmployerPracticeApplicationItem
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public string StudentName { get; set; }

        public string StudentSurname { get; set; }

        public string? StudentPatronymic { get; set; }

        public string? StudentAvatarPath { get; set; }

        public int PracticePeriodId { get; set; }

        public int PracticeOfferId { get; set; }

        public string PracticeOfferTitle { get; set; }

        public int AvailablePlacesCount { get; set; }

        public int AcademicYearStart { get; set; }

        public int AcademicYearEnd => AcademicYearStart + 1;

        public int CourseNumber { get; set; }

        public DateOnly PracticeStartDate { get; set; }

        public DateOnly PracticeEndDate { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}