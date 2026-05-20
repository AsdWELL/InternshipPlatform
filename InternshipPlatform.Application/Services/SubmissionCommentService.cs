using InternshipPlatform.Application.Dtos.SubmissionComment;
using InternshipPlatform.Application.Exceptions.PracticeSubmission;
using InternshipPlatform.Application.Exceptions.StudentPractice;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Services
{
    public class SubmissionCommentService(
        IStudentPracticeRepository studentPracticeRepository,
        ISubmissionCommentRepository commentRepository,
        IUnitOfWork unitOfWork) : ISubmissionCommentService
    {
        private static void ThrowIfSubmissionIsNull (StudentPractice studentPractice)
        {
            if (studentPractice.PracticeSubmission is null)
                throw new PracticeSubmissionNotFoundException();
        }

        private void ThrowIfPracticeNotActivce(PracticePeriod practicePeriod)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (practicePeriod.StartDate > today)
                throw new PracticeHasNotStartedException();

            if (practicePeriod.EndDate < today)
                throw new PracticeAlreadyEndedException();
        }

        public async Task SendSubmissionCommentByEmployer(SendSubmissionCommentByEmployerRequest request)
        {
            var practice = await studentPracticeRepository
                .GetEmployerStudentPracticeDetails(request.EmployerId, request.StudentPracticeId)
                ?? throw new StudentPracticeNotFoundException();

            ThrowIfSubmissionIsNull(practice);

            ThrowIfPracticeNotActivce(practice.PracticePeriod);

            await commentRepository.AddSubmissionComment(request.ToDomain(practice.PracticeSubmission!.Id));

            await unitOfWork.SaveChangesAsync();
        }

        public async Task SendSubmissionCommentByTeacher(SendSubmissionCommentByTeacherRequest request)
        {
            var practice = await studentPracticeRepository
                .GetTeacherStudentPracticeDetails(request.TeacherId, request.StudentPracticeId)
                ?? throw new StudentPracticeNotFoundException();

            ThrowIfSubmissionIsNull(practice);

            ThrowIfPracticeNotActivce(practice.PracticePeriod);

            await commentRepository.AddSubmissionComment(request.ToDomain(practice.PracticeSubmission!.Id));

            await unitOfWork.SaveChangesAsync();
        }
    }
}
