using InternshipPlatform.Api.Authorization;
using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.Vacancy;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.Vacancies
{
    [Authorize(Policy = Policies.ApprovedUser)]
    public class VacanciesController(IVacancyService vacancyService) : AuthorizedUserController
    {
        [HttpGet("api/vacancies/{vacancyId:int}")]
        public async Task<IActionResult> GetVacancyDetails([FromRoute] int vacancyId)
        {
            return Role switch
            {
                Roles.Student => Ok(await vacancyService.GetVacancyDetailsForStudent(UserId, vacancyId)),
                Roles.Employer => Ok(await vacancyService.GetVacancyDetailsForOwner(UserId, vacancyId)),
                _ => Forbid()
            };
        }

        [HttpGet("api/vacancies")]
        public async Task<IActionResult> SearchVacancies([FromQuery] SearchVacancyParameters parameters)
        {
            parameters.StudentId = UserId;
            
            return Ok(await vacancyService.SearchVacancies(parameters));
        }

        [HttpGet("api/companies/{companyId:int}/vacancies")]
        public async Task<IActionResult> GetCompanyVacancies([FromRoute] int companyId)
        {
            return Ok(await vacancyService.GetCompanyVacancies(UserId, companyId));
        }
    }
}
