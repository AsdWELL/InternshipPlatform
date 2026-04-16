using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.Vacancy;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.Vacancies
{
    public class VacanciesController(IVacancyService vacancyService) : AuthorizedUserController
    {
        [HttpGet("api/vacancies/{vacancyId:int}")]
        public async Task<IActionResult> GetVacancyDetails([FromRoute] int vacancyId)
        {
            return Ok(await vacancyService.GetVacancyDetails(UserId, vacancyId));
        }

        [Authorize(Roles = Roles.Student)]
        [HttpGet("api/vacancies")]
        public async Task<IActionResult> SearchVacancies([FromQuery] SearchVacancyParameters parameters)
        {
            return Ok(await vacancyService.SearchVacancies(parameters));
        }

        [Authorize(Roles = Roles.Student)]
        [HttpGet("api/companies/{companyId:int}/vacancies")]
        public async Task<IActionResult> GetCompanyVacancies([FromRoute] int companyId)
        {
            return Ok(await vacancyService.GetCompanyVacancies(companyId));
        }
    }
}
