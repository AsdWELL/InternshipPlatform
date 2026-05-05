namespace InternshipPlatform.Application.Dtos.Vacancy
{
    public class VacancyResult
    {
        public Domain.Entities.Vacancy Vacancy { get; set; }

        public int ApplicationsCount { get; set; }
        
        public int ViewsCount { get; set; }
    }
}
