using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.Resume;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.Resumes
{
    [Authorize(Roles = Roles.Employer)]
    [Route("api/employers/me/")]
    public class ResumeRecommendationsController(IResumeService resumeService) : AuthorizedUserController
    {
        [HttpGet("recommended-resumes")]
        public async Task<IActionResult> GetRecommendedResumes([FromQuery] PageRequestParameters pageParameters)
        {
            var request = new GetRecommendedResumesRequest
            {
                EmployerId = UserId,
                PageIndex = pageParameters.PageIndex,
                PageSize = pageParameters.PageSize
            };

            return Ok(await resumeService.GetRecommendedResumes(request));
        }

        [HttpGet("vacancies/{vacancyId:int}/recommended-resumes")]
        public async Task<IActionResult> GetRecommendedResumesForVacancy(
            [FromRoute] int vacancyId,
            [FromQuery] PageRequestParameters pageParameters)
        {
            var request = new GetRecommendedResumesForVacancyRequest
            {
                EmployerId = UserId,
                VacancyId = vacancyId,
                PageIndex = pageParameters.PageIndex,
                PageSize = pageParameters.PageSize
            };

            return Ok(await resumeService.GetRecommendedResumesForVacancy(request));
        }
    }
}
