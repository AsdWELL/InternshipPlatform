using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.Resume;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.Resumes
{
    [Authorize(Roles = Roles.Student)]
    [Route("api/students/me/resumes")]
    public class StudentResumesController(IResumeService resumeService) : AuthorizedUserController
    {
        [HttpPost]
        public async Task<IActionResult> CreateResume([FromBody] CreateResumeRequest request)
        {
            request.StudentId = UserId;

            return Ok(await resumeService.CreateResume(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentResumes()
        {
            return Ok(await resumeService.GetStudentResumes(UserId));
        }

        [HttpPut("{resumeId:int}")]
        public async Task<IActionResult> UpdateResume([FromRoute] int resumeId, [FromBody] UpdateResumeRequest request)
        {
            request.StudentId = UserId;
            request.Id = resumeId;

            await resumeService.UpdateResume(request);

            return Ok();
        }

        [HttpDelete("{resumeId:int}")]
        public async Task<IActionResult> DeleteResume([FromRoute] int resumeId)
        {
            await resumeService.DeleteResume(UserId, resumeId);

            return Ok();
        }

        [Authorize(Roles = Roles.Student)]
        [HttpPost("{resumeId:int}/copy")]
        public async Task<IActionResult> CopyResume([FromRoute] int resumeId)
        {
            await resumeService.CopyResume(UserId, resumeId);

            return Ok();
        }
    }
}
