using InternshipPlatform.Api.Authorization;
using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.JobApplication;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.JobApplication
{
    [Authorize(Policy = Policies.StudentMustHaveGroup)]
    [Route("api/students/me/applications")]
    public class StudentApplicationsController(IJobApplicationService applicationService) : AuthorizedUserController
    {
        [HttpPost]
        public async Task<IActionResult> CreateJobApplication([FromBody] CreateJobApplicationRequest request)
        {
            request.UserId = UserId;

            return Ok(await applicationService.CreateJobApplicationByStudent(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetMyApplications([FromQuery] GetStudentApplicationsParameters request)
        {
            request.StudentId = UserId;

            return Ok(await applicationService.GetStudentApplications(request));
        }

        [HttpPut("{applicationId:int}")]
        public async Task<IActionResult> UpdateApplicationStatus(
            [FromRoute] int applicationId,
            [FromBody] UpdateApplicationStatusRequest request)
        {
            request.ApplicationId = applicationId;
            request.UserId = UserId;
            request.Role = Roles.Student;

            await applicationService.UpdateApplicationStatus(request);

            return Ok();
        }
    }
}
