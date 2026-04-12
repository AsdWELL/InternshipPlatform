using InternshipPlatform.Application.Dtos.Pagination;
using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.Vacancy
{
    public class GetRecommendedVacanciesRequest : PageRequestParameters
    {
        [JsonIgnore]
        public int StudentId { get; set; }
    }
}
