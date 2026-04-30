using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.Views
{
    public class VacancyViewsController(IVacancyViewService vacancyViewService) : AuthorizedUserController
    {
        [Authorize(Roles = Roles.Employer)]
        [HttpGet("api/employers/me/vacancies/{vacancyId:int}/views")]
        public async Task<IActionResult> GetVacancyViews([FromRoute] int vacancyId)
        {
            return Ok(await vacancyViewService.GetEmployerVacancyViews(UserId, vacancyId));
        }

        [Authorize(Roles = Roles.Student)]
        [HttpGet("api/students/me/views-history")]
        public async Task<IActionResult> GetStudentVacancyViewsHistory()
        {
            return Ok(await vacancyViewService.GetStudentVacancyViewsHistory(UserId));
        }
    }
}
