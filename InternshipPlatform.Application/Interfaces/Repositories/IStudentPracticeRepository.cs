using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IStudentPracticeRepository
    {
        Task AddStudentPractice(StudentPractice studentPractice);

        Task<StudentPractice?> GetCurrentStudentPractice(int studentId);

        Task<StudentPractice?> GetCurrentStudentPracticeForSubmissionUpdate(int studentId);

        Task<StudentPractice?> GetStudentPracticeForStudentAccess(int studentId, int studentPracticeId);

        Task<List<StudentPractice>> GetEmployerStudentPractices(int employerId);

        Task<StudentPractice?> GetEmployerStudentPracticeDetails(int employerId, int studentPracticeId);

        Task<StudentPractice?> GetEmployerStudentPracticeForUpdate(int employerId, int studentPracticeId);

        Task<List<StudentPractice>> GetTeacherStudentPractices(int teacherId);

        Task<StudentPractice?> GetTeacherStudentPracticeDetails(int teacherId, int studentPracticeId);

        Task<StudentPractice?> GetTeacherStudentPracticeForUpdate(int teacherId, int studentPracticeId);

        Task<PracticeMaterial?> GetStudentPracticeMaterial(int studentId, int materialId);

        Task AddPracticeSubmission(PracticeSubmission submission);

        Task AddPracticeSubmissionStatus(PracticeSubmissionStatus status);
    }
}
