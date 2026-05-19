using InternshipPlatform.Application.Dtos.PracticeMaterial;
using InternshipPlatform.Application.Dtos.PracticeSubmission;
using InternshipPlatform.Application.Values;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class StudentPracticeMapper
    {
        private static string GetSubmissionStatusName(PracticeSubmission submission)
        {
            return submission.Status?.Name ?? ((PracticeSubmissionStatuses)submission.StatusId) switch
            {
                PracticeSubmissionStatuses.Submitted => "Отправлено",
                PracticeSubmissionStatuses.RevisionRequired => "Требуются исправления",
                PracticeSubmissionStatuses.CompanyAccepted => "Решение принято",
                PracticeSubmissionStatuses.Graded => "Оценено",
                _ => string.Empty
            };
        }

        public static PracticeSubmission ToPracticeSubmission(
            this UploadPracticeSubmissionRequest request,
            int studentPracticeId,
            string? reportFilePath,
            string? reportFileName,
            string? solutionPath,
            string? solutionFileName)
        {
            return new PracticeSubmission
            {
                StudentPracticeId = studentPracticeId,
                ReportFilePath = reportFilePath,
                ReportFileName = reportFileName,
                SolutionPath = solutionPath,
                SolutionFileName = solutionFileName,
                SolutionUrl = string.IsNullOrWhiteSpace(request.SolutionUrl)
                    ? null
                    : request.SolutionUrl.Trim(),
                UpdatedAt = DateTime.UtcNow,
                StatusId = (int)PracticeSubmissionStatuses.Submitted
            };
        }

        public static PracticeSubmissionResponse ToResponse(this PracticeSubmission submission)
        {
            return new PracticeSubmissionResponse
            {
                Id = submission.Id,
                Status = GetSubmissionStatusName(submission),
                UpdatedAt = submission.UpdatedAt,
                ReviewedAt = submission.ReviewedAt,
                Grade = submission.Grade,
                ReportFileName = submission.ReportFileName,
                SolutionFileName = submission.SolutionFileName,
                SolutionUrl = submission.SolutionUrl
            };
        }

        public static StudentCurrentPracticeResponse ToStudentCurrentPracticeResponse(this StudentPractice practice)
        {
            return new StudentCurrentPracticeResponse
            {
                StudentPracticeId = practice.Id,
                PracticeOfferId = practice.PracticeOfferId,
                PracticeOfferTitle = practice.PracticeOffer.Title,
                PracticeOfferDescription = practice.PracticeOffer.Description,
                CompanyId = practice.PracticeOffer.CompanyId,
                CompanyName = practice.PracticeOffer.Company.Name,
                PracticePeriodId = practice.PracticePeriodId,
                StartDate = practice.PracticePeriod.StartDate,
                EndDate = practice.PracticePeriod.EndDate,
                SupervisorId = practice.PracticePeriod.SupervisorId,
                SupervisorName = practice.PracticePeriod.Supervisor.Name,
                SupervisorSurname = practice.PracticePeriod.Supervisor.Surname,
                SupervisorPatronymic = practice.PracticePeriod.Supervisor.Patronymic,
                Materials = practice.PracticeOffer.Materials
                    .Select(m => new PracticeMaterialResponse
                    {
                        Id = m.Id,
                        FileName = m.FileName
                    })
                    .ToList(),
                Submission = practice.PracticeSubmission?.ToResponse()
            };
        }

        public static EmployerStudentPracticeItem ToEmployerItem(this StudentPractice practice)
        {
            return new EmployerStudentPracticeItem
            {
                StudentPracticeId = practice.Id,
                StudentId = practice.StudentId,
                StudentName = practice.Student.Name,
                StudentSurname = practice.Student.Surname,
                StudentPatronymic = practice.Student.Patronymic,
                GroupName = practice.Student.Group?.Name,
                PracticeOfferId = practice.PracticeOfferId,
                PracticeOfferTitle = practice.PracticeOffer.Title,
                SubmissionStatus = practice.PracticeSubmission?.Status.Name
            };
        }

        public static EmployerStudentPracticeDetails ToEmployerDetails(this StudentPractice practice)
        {
            return new EmployerStudentPracticeDetails
            {
                StudentPracticeId = practice.Id,
                StudentId = practice.StudentId,
                StudentName = practice.Student.Name,
                StudentSurname = practice.Student.Surname,
                StudentPatronymic = practice.Student.Patronymic,
                StudentEmail = practice.Student.User.Email,
                GroupName = practice.Student.Group?.Name,
                PracticeOfferId = practice.PracticeOfferId,
                PracticeOfferTitle = practice.PracticeOffer.Title,
                PracticePeriodId = practice.PracticePeriodId,
                StartDate = practice.PracticePeriod.StartDate,
                EndDate = practice.PracticePeriod.EndDate,
                Submission = practice.PracticeSubmission?.ToResponse()
            };
        }

        public static TeacherStudentPracticeItem ToTeacherItem(this StudentPractice practice)
        {
            return new TeacherStudentPracticeItem
            {
                StudentPracticeId = practice.Id,
                StudentId = practice.StudentId,
                StudentName = practice.Student.Name,
                StudentSurname = practice.Student.Surname,
                StudentPatronymic = practice.Student.Patronymic,
                GroupName = practice.Student.Group!.Name,
                CompanyName = practice.PracticeOffer.Company.Name,
                PracticeOfferTitle = practice.PracticeOffer.Title,
                SubmissionStatus = practice.PracticeSubmission?.Status.Name,
                Grade = practice.PracticeSubmission?.Grade
            };
        }

        public static TeacherStudentPracticeDetails ToTeacherDetails(this StudentPractice practice)
        {
            return new TeacherStudentPracticeDetails
            {
                StudentPracticeId = practice.Id,
                StudentId = practice.StudentId,
                StudentName = practice.Student.Name,
                StudentSurname = practice.Student.Surname,
                StudentPatronymic = practice.Student.Patronymic,
                StudentEmail = practice.Student.User.Email,
                GroupName = practice.Student.Group!.Name,
                CompanyId = practice.PracticeOffer.CompanyId,
                CompanyName = practice.PracticeOffer.Company.Name,
                PracticeOfferId = practice.PracticeOfferId,
                PracticeOfferTitle = practice.PracticeOffer.Title,
                PracticePeriodId = practice.PracticePeriodId,
                StartDate = practice.PracticePeriod.StartDate,
                EndDate = practice.PracticePeriod.EndDate,
                Submission = practice.PracticeSubmission?.ToResponse()
            };
        }
    }
}
