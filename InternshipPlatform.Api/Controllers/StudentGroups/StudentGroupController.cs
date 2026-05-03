using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.StudentGroups
{
    [Authorize(Roles = Roles.Student)]
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
