using InternshipPlatform.Application.Dtos.SubmissionComment;
using InternshipPlatform.Application.Utils;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class SubmissionCommentMapper
    {
        public static SubmissionCommentResponse ToResponseForTeacher(this SubmissionComment comment)
        {
            string sender;
            if (comment.Employer is not null)
                sender = comment.Employer.Company.Name;
            else if (comment.Teacher is not null)
                sender = "Вы";
            else
                throw new InvalidOperationException();

            return new SubmissionCommentResponse
            {
                Id = comment.Id,
                Content = comment.Content,
                Sender = sender
            };
        }

        public static SubmissionCommentResponse ToResponseForEmployer(this SubmissionComment comment)
        {
            string sender;
            if (comment.Employer is not null)
                sender = "Вы";
            else if (comment.Teacher is not null)
                sender = $"{comment.Teacher.Surname} {comment.Teacher.Name}";
            else
                throw new InvalidOperationException();

            return new SubmissionCommentResponse
            {
                Id = comment.Id,
                Content = comment.Content,
                Sender = sender
            };
        }

        public static SubmissionCommentResponse ToResponse(this SubmissionComment comment)
        {
            string sender;
            if (comment.Employer is not null)
                sender = comment.Employer.Company.Name;
            else if (comment.Teacher is not null)
                sender = $"{comment.Teacher.Surname} {comment.Teacher.Name}";
            else
                throw new InvalidOperationException();

            return new SubmissionCommentResponse
            {
                Id = comment.Id,
                Content = comment.Content,
                Sender = sender
            };
        }

        public static SubmissionComment ToDomain(
            this SendSubmissionCommentByEmployerRequest request,
            int practiceSubmissionId)
        {
            return new SubmissionComment
            {
                SenderId = request.EmployerId,
                PracticeSubmissionId = practiceSubmissionId,
                CreatedAt = DateTime.UtcNow,
                Content = StringNormalizer.NormalizeRequired(request.Content)
            };
        }

        public static SubmissionComment ToDomain(
            this SendSubmissionCommentByTeacherRequest request,
            int practiceSubmissionId)
        {
            return new SubmissionComment
            {
                SenderId = request.TeacherId,
                PracticeSubmissionId = practiceSubmissionId,
                CreatedAt = DateTime.UtcNow,
                Content = StringNormalizer.NormalizeRequired(request.Content)
            };
        }
    }
}
