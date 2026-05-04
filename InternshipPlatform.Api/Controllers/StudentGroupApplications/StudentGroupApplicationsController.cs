using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.StudentGroupApplication;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.StudentGroupApplications
{
    [Authorize(Roles = Roles.Student)]
    [Route("api/students/me/group-applications")]
    public class StudentGroupApplicationsController(
        IStudentGroupApplicationService studentGroupApplicationService) 
        : AuthorizedUserController
    {
        [HttpPost]
        public async Task<IActionResult> CreateStudentGroupApplication([FromBody] CreateStudentGroupApplicationRequest request)
        {
            request.StudentId = UserId;

            return Ok(await studentGroupApplicationService.CreateStudentGroupApplication(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentGroupApplication()
        {
            return Ok(await studentGroupApplicationService.GetStudentGroupApplication(UserId));
        }

        [HttpDelete("{applicationId:int}")]
        public async Task<IActionResult> DeleteStudentGroupApplication([FromRoute] int applicationId)
        {
            await studentGroupApplicationService.DeleteStudentGroupApplication(UserId, applicationId);

            return Ok();
        }
    }
}
