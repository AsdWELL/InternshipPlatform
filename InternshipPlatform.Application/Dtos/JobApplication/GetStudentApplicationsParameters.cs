using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.JobApplication
{
    public class GetStudentApplicationsParameters : PageRequestParameters
    {
        [BindNever]
        [JsonIgnore]
        public int StudentId { get; set; }

        public int? ResumeId { get; set; }

        public JobApplicationStatuses? Status { get; set; }
    }
}
