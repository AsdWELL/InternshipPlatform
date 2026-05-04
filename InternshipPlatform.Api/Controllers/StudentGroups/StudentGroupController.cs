using InternshipPlatform.Api.Authorization;
using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.StudentGroups
{
    [Authorize(Policy = Policies.StudentMustHaveGroup)]
    [Route("api/students/me/group")]
    public class StudentGroupController(IStudentGroupService studentGroupService) : AuthorizedUserController
    {
        [HttpGet]
        public async Task<IActionResult> GetMyGroup()
        {
            return Ok(await studentGroupService.GetGroupDetailsForStudent(UserId));
        }
    }
}
