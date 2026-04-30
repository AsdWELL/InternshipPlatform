using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers
{
    public class ResumeViewsController(IResumeViewService resumeViewService) : AuthorizedUserController
    {
        [Authorize(Roles = Roles.Student)]
        [HttpGet("api/students/me/resumes/{resumeId:int}/views")]
        public async Task<IActionResult> GetResumeViews([FromRoute] int resumeId)
        {
            return Ok(await resumeViewService.GetStudentResumeViews(UserId, resumeId));
        }

        [Authorize(Roles = Roles.Employer)]
        [HttpGet("api/employers/me/views-history")]
        public async Task<IActionResult> GetViewsHistory()
        {
            return Ok(await resumeViewService.GetEmployerResumeViewsHistory(UserId));
        }
    }
}
