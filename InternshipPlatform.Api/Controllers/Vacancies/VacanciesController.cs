using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.Vacancy;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.Vacancies
{
    [Route("api/vacancies")]
    public class VacanciesController(IVacancyService vacancyService) : AuthorizedUserController
    {
        [HttpGet("{vacancyId:int}")]
        public async Task<IActionResult> GetVacancyDetails([FromRoute] int vacancyId)
        {
            return Ok(await vacancyService.GetVacancyDetails(UserId, vacancyId));
        }

        [Authorize(Roles = Roles.Student)]
        [HttpGet]
        public async Task<IActionResult> SearchVacancies([FromQuery] SearchVacancyParameters parameters)
        {
            return Ok(await vacancyService.SearchVacancies(parameters));
        }
    }
}
