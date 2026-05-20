using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class SubmissionCommentRepository(InternshipPlatformContext context) : ISubmissionCommentRepository
    {
        public async Task AddSubmissionComment(SubmissionComment comment)
        {
            await context.SubmissionComments.AddAsync(comment);
        }
    }
}
