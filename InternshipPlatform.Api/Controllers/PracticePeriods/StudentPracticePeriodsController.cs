using InternshipPlatform.Api.Authorization;
using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.PracticePeriods
{
    [Authorize(Policy = Policies.StudentMustHaveGroup)]
    [Route("api/students/me/practice-periods")]
    public class StudentPracticePeriodsController(IPracticePeriodService practicePeriodService) : AuthorizedUserController
    {
        [HttpGet]
        public async Task<IActionResult> GetMyPracticePeriods()
        {
            return Ok(await practicePeriodService.GetStudentPracticePeriods(UserId));
        }

        [HttpGet("{practicePeriodId:int}")]
        public async Task<IActionResult> GetMyPracticePeriodDetails([FromRoute] int practicePeriodId)
        {
            return Ok(await practicePeriodService.GetStudentPracticePeriodDetails(UserId, practicePeriodId));
        }
    }
}
