using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.StudentGroupApplications
{
    [Authorize(Roles = Roles.Teacher)]
    [Route("api/curators/me/group-applications")]
    public class CuratorGroupApplicationsController(
        IStudentGroupApplicationService studentGroupApplicationService)
        : AuthorizedUserController
    {
        [HttpGet]
        public async Task<IActionResult> GetCuratorGroupApplications()
        {
            return Ok(await studentGroupApplicationService.GetCuratorGroupApplications(UserId));
        }

        [HttpPut("{applicationId:int}/accept")]
        public async Task<IActionResult> AcceptGroupApplication([FromRoute] int applicationId)
        {
            await studentGroupApplicationService.AcceptGroupApplication(UserId, applicationId);

            return Ok();
        }

        [HttpPut("{applicationId:int}/reject")]
        public async Task<IActionResult> RejectGroupApplication([FromRoute] int applicationId)
        {
            await studentGroupApplicationService.RejectGroupApplication(UserId, applicationId);

            return Ok();
        }
    }
}
