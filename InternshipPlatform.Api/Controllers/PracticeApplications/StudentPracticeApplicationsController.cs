using InternshipPlatform.Api.Authorization;
using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.PracticeApplication;
using InternshipPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.PracticeApplications
{
    [Authorize(Policy = Policies.StudentMustHaveGroup)]
    [Route("api/students/me/practice-applications")]
    public class StudentPracticeApplicationsController(
        IPracticeApplicationService practiceApplicationService) : AuthorizedUserController
    {
        [HttpPost]
        public async Task<IActionResult> CreatePracticeApplication([FromBody] CreatePracticeApplicationRequest request)
        {
            request.StudentId = UserId;

            return Ok(await practiceApplicationService.CreatePracticeApplication(request));
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentPracticeApplication()
        {
            return Ok(await practiceApplicationService.GetCurrentStudentPracticeApplication(UserId));
        }

        [HttpDelete("{applicationId:int}")]
        public async Task<IActionResult> CancelPracticeApplication([FromRoute] int applicationId)
        {
            await practiceApplicationService.CancelPracticeApplication(UserId, applicationId);

            return Ok();
        }
    }
}
