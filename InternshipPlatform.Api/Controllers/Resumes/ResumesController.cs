using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.Resume;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.Resumes
{
    [Route("api/resumes")]
    public class ResumesController(IResumeService resumeService) : AuthorizedUserController
    {
        [HttpGet("{resumeId:int}")]
        public async Task<IActionResult> GetResumeDetails([FromRoute] int resumeId)
        {
            return Ok(await resumeService.GetResumeDetails(UserId, resumeId));
        }

        [Authorize(Roles = Roles.Employer)]
        [HttpGet]
        public async Task<IActionResult> SearchResumes([FromQuery] SearchResumeParameters parameters)
        {
            return Ok(await resumeService.SearchResumes(parameters));
        }
    }
}
