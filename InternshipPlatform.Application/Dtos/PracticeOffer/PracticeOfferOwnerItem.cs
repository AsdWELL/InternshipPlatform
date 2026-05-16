namespace InternshipPlatform.Application.Dtos.PracticeOffer
{
    public class PracticeOfferOwnerItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsRemote { get; set; }

        public string Specialization { get; set; }

        public int MaxStudents { get; set; }

        public string CompanyName { get; set; }

        public string? CompanyLogoPath { get; set; }

        public string? Region { get; set; }

        public bool IsActive { get; set; }
    }
}
