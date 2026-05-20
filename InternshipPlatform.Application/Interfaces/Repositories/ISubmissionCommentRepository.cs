using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface ISubmissionCommentRepository
    {
        Task AddSubmissionComment(SubmissionComment comment);
    }
}
