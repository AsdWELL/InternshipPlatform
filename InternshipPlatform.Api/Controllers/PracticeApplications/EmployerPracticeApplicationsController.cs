using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.PracticeApplications
{
    [Authorize(Roles = Roles.Employer)]
    [Route("api/employers/me/practice-applications")]
    public class EmployerPracticeApplicationsController(
        IPracticeApplicationService practiceApplicationService) : AuthorizedUserController
    {
        [HttpGet]
        public async Task<IActionResult> GetPracticeApplications()
        {
            return Ok(await practiceApplicationService.GetEmployerPracticeApplications(UserId));
        }

        [HttpGet("{applicationId:int}")]
        public async Task<IActionResult> GetPracticeApplicationDetails([FromRoute] int applicationId)
        {
            return Ok(await practiceApplicationService.GetEmployerPracticeApplicationDetails(UserId, applicationId));
        }

        [HttpPut("{applicationId:int}/accept")]
        public async Task<IActionResult> AcceptPracticeApplication([FromRoute] int applicationId)
        {
            await practiceApplicationService.AcceptPracticeApplication(UserId, applicationId);

            return Ok();
        }

        [HttpPut("{applicationId:int}/reject")]
        public async Task<IActionResult> RejectPracticeApplication([FromRoute] int applicationId)
        {
            await practiceApplicationService.RejectPracticeApplication(UserId, applicationId);

            return Ok();
        }
    }
}
