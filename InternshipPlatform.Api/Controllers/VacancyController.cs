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
            return Ok(await vacancyService.GetVacancyDetails(vacancyId));
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
