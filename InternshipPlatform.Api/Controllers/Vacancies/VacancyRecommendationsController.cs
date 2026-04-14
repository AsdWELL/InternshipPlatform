using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.Vacancy;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.Vacancies
{
    [Authorize(Roles = Roles.Student)]
    [Route("api/students/me")]
    public class VacancyRecommendationsController(IVacancyService vacancyService) : AuthorizedUserController
    {
        [HttpGet("recommended-vacancies")]
        public async Task<IActionResult> GetRecommendedVacancies([FromQuery] PageRequestParameters pageParameters)
        {
            var request = new GetRecommendedVacanciesRequest
            {
                StudentId = UserId,
                PageIndex = pageParameters.PageIndex,
                PageSize = pageParameters.PageSize
            };

            return Ok(await vacancyService.GetRecommendedVacancies(request));
        }

        [HttpGet("resumes/{resumeId:int}/recommended-vacancies")]
        public async Task<IActionResult> GetRecommendedVacanciesForResume(
            [FromRoute] int resumeId,
            [FromQuery] PageRequestParameters pageParameters)
        {
            var request = new GetRecommendedVacanciesForResumeRequest
            {
                StudentId = UserId,
                ResumeId = resumeId,
                PageIndex = pageParameters.PageIndex,
                PageSize = pageParameters.PageSize
            };

            return Ok(await vacancyService.GetRecommendedVacanciesForResume(request));
        }
    }
}
