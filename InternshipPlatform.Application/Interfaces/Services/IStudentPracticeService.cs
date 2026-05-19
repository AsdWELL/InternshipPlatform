using InternshipPlatform.Application.Dtos.File;
using InternshipPlatform.Application.Dtos.PracticeSubmission;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IStudentPracticeService
    {
        Task<StudentCurrentPracticeResponse> GetCurrentStudentPractice(int studentId);

        Task<FileDownloadResult> DownloadStudentPracticeMaterial(int studentId, int materialId);

        Task<PracticeSubmissionResponse> UploadStudentPracticeSubmission(int studentId, UploadPracticeSubmissionRequest request);

        Task<FileDownloadResult> DownloadStudentReport(int studentId, int studentPracticeId);

        Task<FileDownloadResult> DownloadStudentSolution(int studentId, int studentPracticeId);

        Task<List<EmployerStudentPracticeItem>> GetEmployerStudentPractices(int employerId);

        Task<EmployerStudentPracticeDetails> GetEmployerStudentPracticeDetails(int employerId, int studentPracticeId);

        Task<FileDownloadResult> DownloadEmployerStudentReport(int employerId, int studentPracticeId);

        Task<FileDownloadResult> DownloadEmployerStudentSolution(int employerId, int studentPracticeId);

        Task AcceptStudentPracticeSubmission(int employerId, int studentPracticeId);

        Task SendStudentPracticeSubmissionToRevision(int employerId, int studentPracticeId);

        Task<List<TeacherStudentPracticeItem>> GetTeacherStudentPractices(int teacherId);

        Task<TeacherStudentPracticeDetails> GetTeacherStudentPracticeDetails(int teacherId, int studentPracticeId);

        Task<FileDownloadResult> DownloadTeacherStudentReport(int teacherId, int studentPracticeId);

        Task<FileDownloadResult> DownloadTeacherStudentSolution(int teacherId, int studentPracticeId);

        Task GradeStudentPractice(int teacherId, int studentPracticeId, GradePracticeSubmissionRequest request);
    }
}