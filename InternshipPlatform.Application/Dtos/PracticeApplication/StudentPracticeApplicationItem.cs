namespace InternshipPlatform.Application.Dtos.PracticeApplication
{
    public class StudentPracticeApplicationItem
    {
        public int Id { get; set; }

        public int PracticePeriodId { get; set; }

        public int PracticeOfferId { get; set; }

        public string PracticeOfferTitle { get; set; }

        public int AvailablePlacesCount { get; set; }

        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string? CompanyLogoPath { get; set; }

        public int AcademicYearStart { get; set; }

        public int AcademicYearEnd => AcademicYearStart + 1;

        public int CourseNumber { get; set; }

        public DateOnly PracticeStartDate { get; set; }

        public DateOnly PracticeEndDate { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}