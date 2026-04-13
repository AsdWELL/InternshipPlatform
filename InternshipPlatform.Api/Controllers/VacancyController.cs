using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.Vacancy;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    public class VacancyController(IVacancyService vacancyService) : AuthorizedUserController
    {
        [Authorize(Roles = Roles.Employer)]
        [HttpPost]
        public async Task<IActionResult> CreateVacancy([FromBody] CreateVacancyRequest request)
        {
            request.EmployerId = UserId;
            
            return Ok(await vacancyService.CreateVacancy(request));
        }

        [Authorize(Roles = Roles.Employer)]
        [HttpGet("my-vacancies")]
        public async Task<IActionResult> GetEmployerVacancies()
        {
            return Ok(await vacancyService.GetEmployerVacancies(UserId));
        }

        [HttpGet("{vacancyId:int}")]
        public async Task<IActionResult> GetVacancyDetails([FromRoute] int vacancyId)
        {
            return Ok(await vacancyService.GetVacancyDetails(UserId, vacancyId));
        }

        [Authorize(Roles = Roles.Student)]
        [HttpGet("recommended")]
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

        [Authorize(Roles = Roles.Student)]
        [HttpGet("recommended/by-resume/{resumeId:int}")]
        public async Task<IActionResult> GetRecommendedVacanciesForResume([FromRoute] int resumeId, [FromQuery] PageRequestParameters pageParameters)
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

        [Authorize(Roles = Roles.Student)]
        [HttpGet("search")]
        public async Task<IActionResult> SearchVacancies([FromQuery] SearchVacancyParameters parameters)
        {
            return Ok(await vacancyService.SearchVacancies(parameters));
        }

        [Authorize(Roles = Roles.Employer)]
        [HttpPut]
        public async Task<IActionResult> UpdateVacancy([FromBody] UpdateVacancyRequest request)
        {
            request.EmployerId = UserId;
            
            await vacancyService.UpdateVacancy(request);

            return Ok();
        }

        [Authorize(Roles = Roles.Employer)]
        [HttpDelete("{vacancyId:int}")]
        public async Task<IActionResult> DeleteVacancy([FromRoute] int vacancyId)
        {
            await vacancyService.DeleteVacancy(UserId, vacancyId);

            return Ok();
        }
    }
}
