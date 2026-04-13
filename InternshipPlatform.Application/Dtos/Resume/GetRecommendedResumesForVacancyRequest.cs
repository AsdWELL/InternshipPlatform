using InternshipPlatform.Application.Dtos.Pagination;
using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.Resume
{
    public class GetRecommendedResumesForVacancyRequest : PageRequestParameters
    {
        [JsonIgnore]
        public int EmployerId { get; set; }

        public int VacancyId { get; set; }
    }
}
