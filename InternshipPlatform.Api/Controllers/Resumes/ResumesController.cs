using InternshipPlatform.Api.Authorization;
using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.Resume;
using InternshipPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.Resumes
{
    [Authorize(Policy = Policies.ApprovedUser)]
    public class ResumesController(IResumeService resumeService) : AuthorizedUserController
    {
        [HttpGet("api/resumes/{resumeId:int}")]
        public async Task<IActionResult> GetResumeDetails([FromRoute] int resumeId)
        {
            return Ok(await resumeService.GetResumeDetails(UserId, resumeId));
        }

        [HttpGet("api/resumes")]
        public async Task<IActionResult> SearchResumes([FromQuery] SearchResumeParameters parameters)
        {
            return Ok(await resumeService.SearchResumes(parameters));
        }

        [HttpGet("api/students/{studentId:int}/resumes")]
        public async Task<IActionResult> GetStudentResumes([FromRoute] int studentId)
        {
            return Ok(await resumeService.GetStudentResumes(studentId));
        }
    }
}
