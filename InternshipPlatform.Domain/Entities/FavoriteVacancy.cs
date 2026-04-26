namespace InternshipPlatform.Domain.Entities
{
    public class FavoriteVacancy
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public int VacancyId { get; set; }

        public Vacancy Vacancy { get; set; }
    }
}
