using InternshipPlatform.Application.Dtos.Pagination;
using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.Vacancy
{
    public class GetRecommendedVacanciesForResumeRequest : PageRequestParameters
    {
        [JsonIgnore]
        public int StudentId { get; set; }

        public int ResumeId { get; set; }
    }
}
