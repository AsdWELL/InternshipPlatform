using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.PracticeSubmission;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.StudentPractices
{
    [Authorize(Roles = Roles.Teacher)]
    [Route("api/teachers/me/student-practices")]
    public class TeacherStudentPracticesController(IStudentPracticeService studentPracticeService) : AuthorizedUserController
    {
        [HttpGet]
        public async Task<IActionResult> GetStudentPractices()
        {
            return Ok(await studentPracticeService.GetTeacherStudentPractices(UserId));
        }

        [HttpGet("{studentPracticeId:int}")]
        public async Task<IActionResult> GetStudentPracticeDetails([FromRoute] int studentPracticeId)
        {
            return Ok(await studentPracticeService.GetTeacherStudentPracticeDetails(UserId, studentPracticeId));
        }

        [HttpGet("{studentPracticeId:int}/report")]
        public async Task<IActionResult> DownloadReport([FromRoute] int studentPracticeId)
        {
            var file = await studentPracticeService.DownloadTeacherStudentReport(UserId, studentPracticeId);

            return File(file.Stream, file.ContentType, file.FileName);
        }

        [HttpGet("{studentPracticeId:int}/solution")]
        public async Task<IActionResult> DownloadSolution([FromRoute] int studentPracticeId)
        {
            var file = await studentPracticeService.DownloadTeacherStudentSolution(UserId, studentPracticeId);

            return File(file.Stream, file.ContentType, file.FileName);
        }

        [HttpPut("{studentPracticeId:int}/grade")]
        public async Task<IActionResult> GradeStudentPractice(
            [FromRoute] int studentPracticeId,
            [FromBody] GradePracticeSubmissionRequest request)
        {
            await studentPracticeService.GradeStudentPractice(UserId, studentPracticeId, request);

            return Ok();
        }
    }
}
