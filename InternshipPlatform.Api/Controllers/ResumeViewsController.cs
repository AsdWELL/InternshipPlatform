using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers
{
    [Authorize(Roles = Roles.Student)]
    public class ResumeViewsController(IResumeViewService resumeViewService) : AuthorizedUserController
    {
        [HttpGet("api/students/me/resumes/{resumeId:int}/views")]
        public async Task<IActionResult> GetResumeViews([FromRoute] int resumeId)
        {
            return Ok(await resumeViewService.GetResumeViews(UserId, resumeId));
        }
    }
}
