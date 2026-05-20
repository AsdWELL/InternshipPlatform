using InternshipPlatform.Application.Dtos.SubmissionComment;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface ISubmissionCommentService
    {
        Task SendSubmissionCommentByEmployer(SendSubmissionCommentByEmployerRequest request);

        Task SendSubmissionCommentByTeacher(SendSubmissionCommentByTeacherRequest request);
    }
}
