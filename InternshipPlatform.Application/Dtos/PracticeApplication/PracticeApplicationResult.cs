namespace InternshipPlatform.Application.Dtos.PracticeApplication
{
    public class PracticeApplicationResult
    {
        public Domain.Entities.PracticeApplication PracticeApplication { get; set; }

        public int AvailablePlaces { get; set; }
    }
}
