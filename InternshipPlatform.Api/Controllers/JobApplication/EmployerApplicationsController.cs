using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.JobApplication;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.JobApplication
{
    [Authorize(Roles = Roles.Employer)]
    [Route("api/employers/me/applications")]
    public class EmployerApplicationsController(IJobApplicationService applicationService) : AuthorizedUserController
    {
        [HttpPost]
        public async Task<IActionResult> CreateJobApplication(CreateJobApplicaionRequest request)
        {
            request.UserId = UserId;

            return Ok(await applicationService.CreateJobApplicationByEmployer(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetMyApplications([FromQuery] GetEmployerApplicationsParameters request)
        {
            request.EmployerId = UserId;

            return Ok(await applicationService.GetEmployerApplications(request));
        }

        [HttpPut("{applicationId:int}")]
        public async Task<IActionResult> UpdateApplicationStatus(
            [FromRoute] int applicationId,
            [FromBody] UpdateApplicationStatusRequest request)
        {
            request.ApplicationId = applicationId;
            request.UserId = UserId;
            request.Role = Roles.Employer;

            await applicationService.UpdateApplicationStatus(request);

            return Ok();
        }
    }
}
