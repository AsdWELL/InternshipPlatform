using InternshipPlatform.Api.Authorization;
using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.PracticeSubmission;
using InternshipPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.StudentPractices
{
    [Authorize(Policy = Policies.StudentMustHaveGroup)]
    [Route("api/students/me/practices")]
    public class StudentPracticesController(IStudentPracticeService studentPracticeService) : AuthorizedUserController
    {
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentStudentPractice()
        {
            return Ok(await studentPracticeService.GetCurrentStudentPractice(UserId));
        }

        [HttpGet("current/materials/{materialId:int}")]
        public async Task<IActionResult> DownloadPracticeMaterial([FromRoute] int materialId)
        {
            var file = await studentPracticeService.DownloadStudentPracticeMaterial(UserId, materialId);

            return File(file.Stream, file.ContentType, file.FileName);
        }

        [HttpPut("current/submission")]
        public async Task<IActionResult> UploadPracticeSubmission([FromForm] UploadPracticeSubmissionRequest request)
        {
            return Ok(await studentPracticeService.UploadStudentPracticeSubmission(UserId, request));
        }

        [HttpGet("{studentPracticeId:int}/report")]
        public async Task<IActionResult> DownloadReport([FromRoute] int studentPracticeId)
        {
            var file = await studentPracticeService.DownloadStudentReport(UserId, studentPracticeId);

            return File(file.Stream, file.ContentType, file.FileName);
        }

        [HttpGet("{studentPracticeId:int}/solution")]
        public async Task<IActionResult> DownloadSolution([FromRoute] int studentPracticeId)
        {
            var file = await studentPracticeService.DownloadStudentSolution(UserId, studentPracticeId);

            return File(file.Stream, file.ContentType, file.FileName);
        }
    }
}
