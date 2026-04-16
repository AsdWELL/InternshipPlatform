using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.Vacancy;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.Vacancies
{
    [Authorize(Roles = Roles.Employer)]
    [Route("api/employers/me/vacancies")]
    public class EmployerVacanciesController(IVacancyService vacancyService) : AuthorizedUserController
    {
        [HttpPost]
        public async Task<IActionResult> CreateVacancy([FromBody] CreateVacancyRequest request)
        {
            request.EmployerId = UserId;

            return Ok(await vacancyService.CreateVacancy(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetMyVacancies()
        {
            return Ok(await vacancyService.GetEmployerVacancies(UserId));
        }

        [HttpPut("{vacancyId:int}")]
        public async Task<IActionResult> UpdateVacancy([FromRoute] int vacancyId, [FromBody] UpdateVacancyRequest request)
        {
            request.EmployerId = UserId;
            request.Id = vacancyId;

            await vacancyService.UpdateVacancy(request);

            return Ok();
        }

        [HttpDelete("{vacancyId:int}")]
        public async Task<IActionResult> DeleteVacancy([FromRoute] int vacancyId)
        {
            await vacancyService.DeleteVacancy(UserId, vacancyId);

            return Ok();
        }
    }
}
