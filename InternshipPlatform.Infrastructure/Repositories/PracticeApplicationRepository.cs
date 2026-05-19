using InternshipPlatform.Application.Dtos.PracticeApplication;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class PracticeApplicationRepository(InternshipPlatformContext context) : IPracticeApplicationRepository
    {
        public async Task AddPracticeApplication(PracticeApplication application)
        {
            await context.PracticeApplications.AddAsync(application);
        }

        public async Task<PracticeApplicationResult?> GetCurrentStudentPracticeApplication(int studentId)
        {
            return await context.PracticeApplications
                .AsNoTracking()
                .Where(a => a.StudentId == studentId)
                .Include(a => a.PracticePeriod)
                .Include(a => a.PracticeOffer)
                    .ThenInclude(po => po.Company)
                .Select(a => new PracticeApplicationResult
                {
                    PracticeApplication = a,
                    AvailablePlaces = a.PracticeOffer.MaxStudents -
                        context.StudentPractices.Count(sp =>
                            sp.PracticeOfferId == a.PracticeOfferId)
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> HasActivePracticeApplication(int studentId)
        {
            return await context.PracticeApplications
                .AsNoTracking()
                .AnyAsync(a => a.StudentId == studentId);
        }

        public async Task<PracticeApplication?> GetStudentPracticeApplicationForUpdate(int studentId, int applicationId)
        {
            return await context.PracticeApplications
                .FirstOrDefaultAsync(a => a.Id == applicationId && a.StudentId == studentId);
        }

        public async Task<List<PracticeApplicationResult>> GetEmployerPracticeApplications(int employerId)
        {
            return await context.PracticeApplications
                .AsNoTracking()
                .Where(a => context.EmployerProfiles
                    .AsNoTracking()
                    .Any(ep =>
                        ep.UserId == employerId &&
                        ep.CompanyId == a.PracticeOffer.CompanyId))
                .Include(a => a.Student)
                .Include(a => a.PracticePeriod)
                .Include(a => a.PracticeOffer)
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new PracticeApplicationResult
                {
                    PracticeApplication = a,
                    AvailablePlaces = a.PracticeOffer.MaxStudents -
                        context.StudentPractices.Count(sp =>
                            sp.PracticeOfferId == a.PracticeOfferId)
                })
                .ToListAsync();
        }

        public async Task<PracticeApplicationResult?> GetEmployerPracticeApplicationDetails(int employerId, int applicationId)
        {
            return await context.PracticeApplications
                .AsNoTracking()
                .Where(a => a.Id == applicationId)
                .Where(a => context.EmployerProfiles
                    .AsNoTracking()
                    .Any(ep =>
                        ep.UserId == employerId &&
                        ep.CompanyId == a.PracticeOffer.CompanyId))
                .Include(a => a.Student)
                    .ThenInclude(s => s.User)
                .Include(a => a.Student)
                    .ThenInclude(s => s.Group)
                .Include(a => a.PracticePeriod)
                .Include(a => a.PracticeOffer)
                .Select(a => new PracticeApplicationResult
                {
                    PracticeApplication = a,
                    AvailablePlaces = a.PracticeOffer.MaxStudents -
                        context.StudentPractices.Count(sp =>
                            sp.PracticeOfferId == a.PracticeOfferId)
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PracticeApplicationResult?> GetEmployerPracticeApplicationForUpdate(int employerId, int applicationId)
        {
            return await context.PracticeApplications
                .Where(a => a.Id == applicationId)
                .Where(a => context.EmployerProfiles
                    .AsNoTracking()
                    .Any(ep =>
                        ep.UserId == employerId &&
                        ep.CompanyId == a.PracticeOffer.CompanyId))
                .Select(a => new PracticeApplicationResult
                {
                    PracticeApplication = a,
                    AvailablePlaces = a.PracticeOffer.MaxStudents -
                        context.StudentPractices.Count(sp =>
                            sp.PracticeOfferId == a.PracticeOfferId)
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> HasPracticeOfferAvailablePlaces(int practiceOfferId)
        {
            var practiceOffer = await context.PracticeOffers
                .AsNoTracking()
                .Where(po => po.Id == practiceOfferId && po.IsActive)
                .Select(po => new
                {
                    po.MaxStudents,
                    EnrolledStudentsCount = context.StudentPractices
                        .Count(sp => sp.PracticeOfferId == practiceOfferId)
                })
                .FirstOrDefaultAsync();

            if (practiceOffer is null)
                return false;

            var availablePlaces = Math.Max(0, practiceOffer.MaxStudents - practiceOffer.EnrolledStudentsCount);

            return availablePlaces > 0;
        }

        public async Task<bool> IsStudentAlreadyEnrolledInPracticePeriod(
            int studentId,
            int practicePeriodId)
        {
            return await context.StudentPractices
                .AsNoTracking()
                .AnyAsync(sp =>
                    sp.StudentId == studentId &&
                    sp.PracticePeriodId == practicePeriodId);
        }

        public async Task<List<PracticeApplication>> GetPracticeOfferApplicationsForUpdate(int practiceOfferId)
        {
            return await context.PracticeApplications
                .Where(a => a.PracticeOfferId == practiceOfferId)
                .ToListAsync();
        }

        public async Task DeletePracticeApplication(int applicationId)
        {
            var application = await context.PracticeApplications.FindAsync(applicationId);

            if (application is null)
                return;

            context.PracticeApplications.Remove(application);
        }

        public Task DeletePracticeApplications(List<PracticeApplication> applications)
        {
            context.PracticeApplications.RemoveRange(applications);

            return Task.CompletedTask;
        }
    }
}
