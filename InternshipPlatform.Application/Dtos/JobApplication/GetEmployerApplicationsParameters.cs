using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.JobApplication
{
    public class GetEmployerApplicationsParameters : PageRequestParameters
    {
        [BindNever]
        [JsonIgnore]
        public int EmployerId { get; set; }

        public int? VacancyId { get; set; }

        public JobApplicationStatuses? Status { get; set; }
    }
}
