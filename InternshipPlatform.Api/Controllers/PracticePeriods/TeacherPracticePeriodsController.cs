using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.PracticePeriods
{
    [Authorize(Roles = Roles.Teacher)]
    [Route("api/teachers/me/practice-periods")]
    public class TeacherPracticePeriodsController(
        IPracticePeriodService practicePeriodService) : AuthorizedUserController
    {
        [HttpGet("groups")]
        public async Task<IActionResult> GetCurrentYearPracticeGroups()
        {
            return Ok(await practicePeriodService.GetTeacherCurrentYearPracticeGroups(UserId));
        }

        [HttpGet("{practicePeriodId:int}/groups/{groupId:int}")]
        public async Task<IActionResult> GetPracticeGroupDetails([FromRoute] int practicePeriodId, [FromRoute] int groupId)
        {
            return Ok(await practicePeriodService.GetTeacherPracticeGroupDetails(UserId, practicePeriodId, groupId));
        }
    }
}
