using InternshipPlatform.Application.Dtos.File;
using InternshipPlatform.Application.Dtos.PracticeSubmission;
using InternshipPlatform.Application.Exceptions.PracticeMaterial;
using InternshipPlatform.Application.Exceptions.PracticeSubmission;
using InternshipPlatform.Application.Exceptions.StudentPractice;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Notifiers;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;
using InternshipPlatform.Application.Values;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Services
{
    public class StudentPracticeService(
        IStudentPracticeRepository studentPracticeRepository,
        IFileStorageService fileStorageService,
        IStudentPracticeNotifier studentPracticeNotifier,
        IUnitOfWork unitOfWork) : IStudentPracticeService
    {
        private static bool CanStudentUpdateSubmission(PracticeSubmission submission)
        {
            return submission.StatusId != (int)PracticeSubmissionStatuses.CompanyAccepted &&
                   submission.StatusId != (int)PracticeSubmissionStatuses.Graded;
        }

        private static PracticeSubmission GetSubmissionOrThrow(StudentPractice studentPractice)
        {
            return studentPractice.PracticeSubmission ?? throw new PracticeSubmissionNotFoundException();
        }

        private async Task<FileDownloadResult> GetFileOrThrow(string? filePath, string? fileName)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new PracticeSubmissionFileNotFoundException();

            var file = await fileStorageService.GetFile(filePath)
                ?? throw new PracticeSubmissionFileNotFoundException();

            if (!string.IsNullOrWhiteSpace(fileName))
                file.FileName = fileName;

            return file;
        }

        private void ThrowIfPracticeNotActivce(PracticePeriod practicePeriod)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (practicePeriod.StartDate > today)
                throw new PracticeHasNotStartedException();

            if (practicePeriod.EndDate < today)
                throw new PracticeAlreadyEndedException();
        }

        public async Task<StudentCurrentPracticeResponse> GetCurrentStudentPractice(int studentId)
        {
            var practice = await studentPracticeRepository.GetCurrentStudentPractice(studentId)
                ?? throw new StudentPracticeNotFoundException();

            return practice.ToStudentCurrentPracticeResponse();
        }

        public async Task<FileDownloadResult> DownloadStudentPracticeMaterial(int studentId, int materialId)
        {
            var material = await studentPracticeRepository.GetStudentPracticeMaterial(studentId, materialId)
                ?? throw new PracticeMaterialNotFoundException();

            var file = await fileStorageService.GetFile(material.FilePath)
                ?? throw new PracticeMaterialNotFoundException();

            file.FileName = material.FileName;

            return file;
        }

        public async Task<PracticeSubmissionResponse> UploadStudentPracticeSubmission(
            int studentId,
            UploadPracticeSubmissionRequest request)
        {
            var practice = await studentPracticeRepository
                .GetCurrentStudentPracticeForSubmissionUpdate(studentId)
                ?? throw new StudentPracticeNotFoundException();

            ThrowIfPracticeNotActivce(practice.PracticePeriod);
            
            var oldReportPath = practice.PracticeSubmission?.ReportFilePath;
            var oldSolutionPath = practice.PracticeSubmission?.SolutionPath;

            if (practice.PracticeSubmission is not null &&
                !CanStudentUpdateSubmission(practice.PracticeSubmission))
                throw new PracticeSubmissionCannotBeUpdatedException();

            SavedFileResult? savedReport = null;
            SavedFileResult? savedSolution = null;

            try
            {
                if (request.ReportFile is not null)
                {
                    await using var reportStream = request.ReportFile.OpenReadStream();
                    savedReport = await fileStorageService.SaveStudentReport(
                        reportStream,
                        request.ReportFile.FileName,
                        Path.GetExtension(request.ReportFile.FileName));
                }
                
                if (request.SolutionFile is not null)
                {
                    await using var solutionStream = request.SolutionFile.OpenReadStream();
                    savedSolution = await fileStorageService.SaveStudentSolution(
                        solutionStream,
                        request.SolutionFile.FileName,
                        Path.GetExtension(request.SolutionFile.FileName));
                }

                if (practice.PracticeSubmission is null)
                {
                    practice.PracticeSubmission = request.ToPracticeSubmission(
                        practice.Id,
                        savedReport?.RelativePath,
                        savedReport?.OriginalFileName,
                        savedSolution?.RelativePath,
                        savedSolution?.OriginalFileName);

                    await studentPracticeRepository.AddPracticeSubmission(practice.PracticeSubmission);
                }
                else
                {
                    practice.PracticeSubmission.ReportFilePath = savedReport?.RelativePath;
                    practice.PracticeSubmission.ReportFileName = savedReport?.OriginalFileName;
                    practice.PracticeSubmission.SolutionPath = savedSolution?.RelativePath;
                    practice.PracticeSubmission.SolutionFileName = savedSolution?.OriginalFileName;
                    practice.PracticeSubmission.SolutionUrl = string.IsNullOrWhiteSpace(request.SolutionUrl)
                        ? null
                        : request.SolutionUrl.Trim();
                    practice.PracticeSubmission.UpdatedAt = DateTime.UtcNow;
                    practice.PracticeSubmission.ReviewedAt = null;
                    practice.PracticeSubmission.Grade = null;
                    practice.PracticeSubmission.StatusId = (int)PracticeSubmissionStatuses.Submitted;
                }

                await unitOfWork.SaveChangesAsync();
            }
            catch
            {
                await fileStorageService.DeleteFileIfExists(savedReport?.RelativePath);
                await fileStorageService.DeleteFileIfExists(savedSolution?.RelativePath);
                throw;
            }

            if (request.ReportFile is not null)
                await fileStorageService.DeleteFileIfExists(oldReportPath);
            if (request.SolutionFile is not null || request.SolutionUrl is not null)
                await fileStorageService.DeleteFileIfExists(oldSolutionPath);

            return practice.PracticeSubmission.ToResponse();
        }

        public async Task<FileDownloadResult> DownloadStudentReport(int studentId, int studentPracticeId)
        {
            var practice = await studentPracticeRepository
                .GetStudentPracticeForStudentAccess(studentId, studentPracticeId)
                ?? throw new StudentPracticeNotFoundException();

            var submission = GetSubmissionOrThrow(practice);

            return await GetFileOrThrow(submission.ReportFilePath, submission.ReportFileName);
        }

        public async Task<FileDownloadResult> DownloadStudentSolution(int studentId, int studentPracticeId)
        {
            var practice = await studentPracticeRepository
                .GetStudentPracticeForStudentAccess(studentId, studentPracticeId)
                ?? throw new StudentPracticeNotFoundException();

            var submission = GetSubmissionOrThrow(practice);

            return await GetFileOrThrow(submission.SolutionPath, submission.SolutionFileName);
        }

        public async Task<List<EmployerStudentPracticeItem>> GetEmployerStudentPractices(int employerId)
        {
            var practices = await studentPracticeRepository.GetEmployerStudentPractices(employerId);

            return practices
                .Select(p => p.ToEmployerItem())
                .ToList();
        }

        public async Task<EmployerStudentPracticeDetails> GetEmployerStudentPracticeDetails(int employerId, int studentPracticeId)
        {
            var practice = await studentPracticeRepository
                .GetEmployerStudentPracticeDetails(employerId, studentPracticeId)
                ?? throw new StudentPracticeNotFoundException();

            return practice.ToEmployerDetails();
        }

        public async Task<FileDownloadResult> DownloadEmployerStudentReport(int employerId, int studentPracticeId)
        {
            var practice = await studentPracticeRepository
                .GetEmployerStudentPracticeDetails(employerId, studentPracticeId)
                ?? throw new StudentPracticeNotFoundException();

            var submission = GetSubmissionOrThrow(practice);

            return await GetFileOrThrow(submission.ReportFilePath, submission.ReportFileName);
        }

        public async Task<FileDownloadResult> DownloadEmployerStudentSolution(int employerId, int studentPracticeId)
        {
            var practice = await studentPracticeRepository
                .GetEmployerStudentPracticeDetails(employerId, studentPracticeId)
                ?? throw new StudentPracticeNotFoundException();

            var submission = GetSubmissionOrThrow(practice);

            return await GetFileOrThrow(submission.SolutionPath, submission.SolutionFileName);
        }

        public async Task AcceptStudentPracticeSubmission(int employerId, int studentPracticeId)
        {
            var practice = await studentPracticeRepository
                .GetEmployerStudentPracticeForUpdate(employerId, studentPracticeId)
                ?? throw new StudentPracticeNotFoundException();

            var submission = GetSubmissionOrThrow(practice);

            if (submission.StatusId == (int)PracticeSubmissionStatuses.Graded)
                throw new PracticeSubmissionAlreadyGradedException();

            submission.StatusId = (int)PracticeSubmissionStatuses.CompanyAccepted;
            submission.ReviewedAt = DateTime.UtcNow;

            await unitOfWork.SaveChangesAsync();

            await studentPracticeNotifier.NotifyStudentPracticeAccepted(
                practice.Student.User.Email,
                practice.PracticeOffer.Company.Name);
        }

        public async Task SendStudentPracticeSubmissionToRevision(int employerId, int studentPracticeId)
        {
            var practice = await studentPracticeRepository
                .GetEmployerStudentPracticeForUpdate(employerId, studentPracticeId)
                ?? throw new StudentPracticeNotFoundException();

            var submission = GetSubmissionOrThrow(practice);

            if (submission.StatusId == (int)PracticeSubmissionStatuses.Graded)
                throw new PracticeSubmissionAlreadyGradedException();

            submission.StatusId = (int)PracticeSubmissionStatuses.RevisionRequired;
            submission.ReviewedAt = DateTime.UtcNow;

            await unitOfWork.SaveChangesAsync();

            await studentPracticeNotifier.NotifyStudentPracticeRejected(
                practice.Student.User.Email,
                practice.PracticeOffer.Company.Name);
        }

        public async Task<List<TeacherStudentPracticeItem>> GetTeacherStudentPractices(int teacherId)
        {
            var practices = await studentPracticeRepository.GetTeacherStudentPractices(teacherId);

            return practices
                .Select(p => p.ToTeacherItem())
                .ToList();
        }

        public async Task<TeacherStudentPracticeDetails> GetTeacherStudentPracticeDetails(int teacherId, int studentPracticeId)
        {
            var practice = await studentPracticeRepository
                .GetTeacherStudentPracticeDetails(teacherId, studentPracticeId)
                ?? throw new StudentPracticeNotFoundException();

            return practice.ToTeacherDetails();
        }

        public async Task<FileDownloadResult> DownloadTeacherStudentReport(int teacherId, int studentPracticeId)
        {
            var practice = await studentPracticeRepository
                .GetTeacherStudentPracticeDetails(teacherId, studentPracticeId)
                ?? throw new StudentPracticeNotFoundException();

            var submission = GetSubmissionOrThrow(practice);

            return await GetFileOrThrow(submission.ReportFilePath, submission.ReportFileName);
        }

        public async Task<FileDownloadResult> DownloadTeacherStudentSolution(int teacherId, int studentPracticeId)
        {
            var practice = await studentPracticeRepository
                .GetTeacherStudentPracticeDetails(teacherId, studentPracticeId)
                ?? throw new StudentPracticeNotFoundException();

            var submission = GetSubmissionOrThrow(practice);

            return await GetFileOrThrow(submission.SolutionPath, submission.SolutionFileName);
        }

        public async Task GradeStudentPractice(int teacherId, int studentPracticeId, GradePracticeSubmissionRequest request)
        {
            var practice = await studentPracticeRepository
                .GetTeacherStudentPracticeForUpdate(teacherId, studentPracticeId)
                ?? throw new StudentPracticeNotFoundException();

            var submission = GetSubmissionOrThrow(practice);

            if (submission.StatusId != (int)PracticeSubmissionStatuses.CompanyAccepted
                && submission.StatusId != (int)PracticeSubmissionStatuses.Graded)
                throw new PracticeSubmissionCannotBeGradedException();

            submission.Grade = request.Grade;
            submission.StatusId = (int)PracticeSubmissionStatuses.Graded;
            submission.ReviewedAt = DateTime.UtcNow;

            await unitOfWork.SaveChangesAsync();

            await studentPracticeNotifier.NotifyStudentPracticeGraded(practice.Student.User.Email, request.Grade);
        }
    }
}
