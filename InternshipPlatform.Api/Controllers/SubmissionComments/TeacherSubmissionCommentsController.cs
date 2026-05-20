using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.SubmissionComment;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.SubmissionComments
{
    [Authorize(Roles = Roles.Teacher)]
    [Route("api/teachers/me/student-practices/{studentPracticeId:int}/submission/comments")]
    public class TeacherSubmissionCommentsController(ISubmissionCommentService commentService) : AuthorizedUserController
    {
        [HttpPost]
        public async Task<IActionResult> SendSubmissionComment(
            [FromRoute] int studentPracticeId,
            [FromBody] SendSubmissionCommentByTeacherRequest request)
        {
            request.StudentPracticeId = studentPracticeId;
            request.TeacherId = UserId;

            await commentService.SendSubmissionCommentByTeacher(request);

            return Ok();
        }
    }
}
