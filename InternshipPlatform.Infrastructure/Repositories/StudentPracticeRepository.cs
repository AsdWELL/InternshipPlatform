using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Utils;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class StudentPracticeRepository(InternshipPlatformContext context) : IStudentPracticeRepository
    {
        public async Task AddStudentPractice(StudentPractice studentPractice)
        {
            await context.StudentPractices.AddAsync(studentPractice);
        }

        public async Task<StudentPractice?> GetCurrentStudentPractice(int studentId)
        {
            var semesterDates = SemesterDatesUtils.GetCurrentSemesterDates();

            if (semesterDates is null)
                return null;

            return await context.StudentPractices
                .AsNoTracking()
                .Where(sp => sp.StudentId == studentId)
                .Where(sp =>
                    sp.PracticePeriod.StartDate >= semesterDates.Start &&
                    sp.PracticePeriod.StartDate <= semesterDates.End)
                .Include(sp => sp.PracticeOffer)
                    .ThenInclude(po => po.Company)
                .Include(sp => sp.PracticeOffer)
                    .ThenInclude(po => po.Materials)
                .Include(sp => sp.PracticePeriod)
                    .ThenInclude(pp => pp.Supervisor)
                .Include(sp => sp.PracticeSubmission)
                    .ThenInclude(ps => ps!.Status)
                .Include(sp => sp.PracticeSubmission)
                    .ThenInclude(ps => ps!.Comments)
                        .ThenInclude(c => c.Employer)
                            .ThenInclude(ep => ep!.Company)
                .Include(sp => sp.PracticeSubmission)
                    .ThenInclude(ps => ps!.Comments)
                        .ThenInclude(c => c.Teacher)
                .FirstOrDefaultAsync();
        }

        public async Task<StudentPractice?> GetCurrentStudentPracticeForSubmissionUpdate(int studentId)
        {
            var semesterDates = SemesterDatesUtils.GetCurrentSemesterDates();

            if (semesterDates is null)
                return null;

            return await context.StudentPractices
                .Where(sp => sp.StudentId == studentId)
                .Where(sp =>
                    sp.PracticePeriod.StartDate >= semesterDates.Start &&
                    sp.PracticePeriod.StartDate <= semesterDates.End)
                .Include(sp => sp.PracticeSubmission)
                    .ThenInclude(ps => ps!.Status)
                .Include(sp => sp.PracticePeriod)
                .FirstOrDefaultAsync();
        }

        public async Task<StudentPractice?> GetStudentPracticeForStudentAccess(int studentId, int studentPracticeId)
        {
            return await context.StudentPractices
                .AsNoTracking()
                .Where(sp => sp.Id == studentPracticeId && sp.StudentId == studentId)
                .Include(sp => sp.PracticeSubmission)
                    .ThenInclude(ps => ps!.Status)
                .FirstOrDefaultAsync();
        }

        public async Task<List<StudentPractice>> GetEmployerStudentPractices(int employerId)
        {
            return await context.StudentPractices
                .AsNoTracking()
                .Where(sp => context.EmployerProfiles
                    .AsNoTracking()
                    .Any(ep =>
                        ep.UserId == employerId &&
                        ep.CompanyId == sp.PracticeOffer.CompanyId))
                .Include(sp => sp.Student)
                    .ThenInclude(s => s.Group)
                .Include(sp => sp.PracticeOffer)
                .Include(sp => sp.PracticeSubmission)
                    .ThenInclude(ps => ps!.Status)
                .OrderBy(sp => sp.PracticePeriod.StartDate)
                .ThenBy(sp => sp.Student.Surname)
                .ToListAsync();
        }

        public async Task<StudentPractice?> GetEmployerStudentPracticeDetails(int employerId, int studentPracticeId)
        {
            return await context.StudentPractices
                .AsNoTracking()
                .Where(sp => sp.Id == studentPracticeId)
                .Where(sp => context.EmployerProfiles
                    .AsNoTracking()
                    .Any(ep =>
                        ep.UserId == employerId &&
                        ep.CompanyId == sp.PracticeOffer.CompanyId))
                .Include(sp => sp.Student)
                    .ThenInclude(s => s.User)
                .Include(sp => sp.Student)
                    .ThenInclude(s => s.Group)
                .Include(sp => sp.PracticeOffer)
                .Include(sp => sp.PracticePeriod)
                .Include(sp => sp.PracticeSubmission)
                    .ThenInclude(ps => ps!.Status)
                .Include(sp => sp.PracticeSubmission)
                    .ThenInclude(ps => ps!.Comments)
                        .ThenInclude(c => c.Employer)
                            .ThenInclude(ep => ep!.Company)
                .Include(sp => sp.PracticeSubmission)
                    .ThenInclude(ps => ps!.Comments)
                        .ThenInclude(c => c.Teacher)
                .FirstOrDefaultAsync();
        }

        public async Task<StudentPractice?> GetEmployerStudentPracticeForUpdate(int employerId, int studentPracticeId)
        {
            return await context.StudentPractices
                .Where(sp => sp.Id == studentPracticeId)
                .Where(sp => context.EmployerProfiles
                    .Any(ep =>
                        ep.UserId == employerId &&
                        ep.CompanyId == sp.PracticeOffer.CompanyId))
                .Include(sp => sp.PracticeSubmission)
                    .ThenInclude(ps => ps!.Status)
                .Include(sp => sp.Student)
                    .ThenInclude(st => st.User)
                .Include(sp => sp.PracticeOffer)
                    .ThenInclude(po => po.Company)
                .FirstOrDefaultAsync();
        }

        public async Task<List<StudentPractice>> GetTeacherStudentPractices(int teacherId)
        {
            return await context.StudentPractices
                .AsNoTracking()
                .Where(sp => sp.PracticePeriod.SupervisorId == teacherId)
                .Include(sp => sp.Student)
                    .ThenInclude(s => s.Group)
                .Include(sp => sp.PracticeOffer)
                    .ThenInclude(po => po.Company)
                .Include(sp => sp.PracticeSubmission)
                    .ThenInclude(ps => ps!.Status)
                .OrderBy(sp => sp.PracticePeriod.StartDate)
                .ThenBy(sp => sp.Student.Surname)
                .ToListAsync();
        }

        public async Task<StudentPractice?> GetTeacherStudentPracticeDetails(int teacherId, int studentPracticeId)
        {
            return await context.StudentPractices
                .AsNoTracking()
                .Where(sp => sp.Id == studentPracticeId)
                .Where(sp => sp.PracticePeriod.SupervisorId == teacherId)
                .Include(sp => sp.Student)
                    .ThenInclude(s => s.User)
                .Include(sp => sp.Student)
                    .ThenInclude(s => s.Group)
                .Include(sp => sp.PracticeOffer)
                    .ThenInclude(po => po.Company)
                .Include(sp => sp.PracticePeriod)
                .Include(sp => sp.PracticeSubmission)
                    .ThenInclude(ps => ps!.Status)
                .Include(sp => sp.PracticeSubmission)
                    .ThenInclude(ps => ps!.Comments)
                        .ThenInclude(c => c.Employer)
                            .ThenInclude(ep => ep!.Company)
                .Include(sp => sp.PracticeSubmission)
                    .ThenInclude(ps => ps!.Comments)
                        .ThenInclude(c => c.Teacher)
                .FirstOrDefaultAsync();
        }

        public async Task<StudentPractice?> GetTeacherStudentPracticeForUpdate(int teacherId, int studentPracticeId)
        {
            return await context.StudentPractices
                .Where(sp => sp.Id == studentPracticeId)
                .Where(sp => sp.PracticePeriod.SupervisorId == teacherId)
                .Include(sp => sp.PracticeSubmission)
                    .ThenInclude(ps => ps!.Status)
                .Include(sp => sp.Student)
                    .ThenInclude(s => s.User)
                .Include(sp => sp.PracticeOffer)
                    .ThenInclude(po => po.Company)
                .FirstOrDefaultAsync();
        }

        public async Task<PracticeMaterial?> GetStudentPracticeMaterial(int studentId, int materialId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            return await context.PracticeMaterials
                .AsNoTracking()
                .Where(pm => pm.Id == materialId)
                .Where(pm => context.StudentPractices
                    .AsNoTracking()
                    .Any(sp =>
                        sp.StudentId == studentId &&
                        sp.PracticeOfferId == pm.PracticeOfferId &&
                        sp.PracticePeriod.StartDate <= today &&
                        sp.PracticePeriod.EndDate >= today))
                .FirstOrDefaultAsync();
        }

        public async Task AddPracticeSubmission(PracticeSubmission submission)
        {
            await context.PracticeSubmissions.AddAsync(submission);
        }
    }
}
