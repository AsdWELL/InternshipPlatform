using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.Resume;
using InternshipPlatform.Application.Exceptions.Company;
using InternshipPlatform.Application.Exceptions.Resume;
using InternshipPlatform.Application.Exceptions.Specialization;
using InternshipPlatform.Application.Exceptions.Vacancy;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;
using InternshipPlatform.Application.Utils;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Services
{
    public class ResumeService(
        IResumeRepository resumeRepository,
        ISkillRepository skillRepository,
        ISpecializationRepository specializationRepository,
        ICompanyRepository companyRepository,
        IVacancyRepository vacancyRepository,
        IResumeViewRepository resumeViewRepository,
        IUnitOfWork unitOfWork) : IResumeService
    {
        private async Task<List<Skill>> TryGetSkillListByIds(List<int> skillIds)
        {
            skillIds = skillIds.Distinct().ToList();
            
            var skills = await skillRepository.GetSkillsByIds(skillIds);

            if (skillIds.Count != skills.Count)
                throw new InvalidResumeSkillsException();

            return skills;
        }
        
        private async Task ThrowIfSpecializationNotExists(int id)
        {
            if (!await specializationRepository.IsSpecializationExists(id))
                throw new InvalidSpecializationException();
        }

        private async Task ThrowIfStudentDoesNotOwnResume(int studentId, int resumeId)
        {
            if (!await resumeRepository.IsStudentOwnsResume(studentId, resumeId))
                throw new ResumeNotFoundException();
        }

        private async Task ThrowIfWorkExperienceNotExists(int workExperienceId)
        {
            if (!await resumeRepository.IsWorkExperienceExists(workExperienceId))
                throw new WorkExperienceNotFoundException();
        }

        private async Task ThrowIfWorkExperienceNotBelongsResume(int resumeId, int workExperienceId)
        {
            if (!await resumeRepository.IsWorkExperienceBelongsToResume(resumeId, workExperienceId))
                throw new WorkExperienceNotFoundException();
        }

        private async Task<int> GetCompanyIdByEmployerIdOrThrow(int employerId)
        {
            var company = await companyRepository.GetCompanyByEmployerId(employerId)
                ?? throw new CompanyNotFoundException();

            return company.Id;
        }

        public async Task<int> CreateResume(CreateResumeRequest request)
        {
            var skills = await TryGetSkillListByIds(request.SkillIds);            

            await ThrowIfSpecializationNotExists(request.SpecializationId);

            var resume = request.ToDomain(skills);
            await resumeRepository.AddResume(resume);

            await unitOfWork.SaveChangesAsync();

            return resume.Id;
        }

        public async Task<List<ResumeOwnerItem>> GetStudentResumes(int studentId)
        {
            var resumes = await resumeRepository.GetStudentResumes(studentId);

            var viewsCount = await resumeViewRepository.GetResumesViewsCount(studentId);

            return resumes.Select(r => r.ToOwnerItem(viewsCount.GetValueOrDefault(r.Id))).ToList();
        }

        public async Task<ResumeDetails> GetResumeDetails(int userId, int resumeId)
        {
            var resume = await resumeRepository.GetResumeById(resumeId);

            if (resume is null || (!resume.IsActive && resume.StudentId != userId))
                throw new ResumeNotFoundException();

            if (resume.StudentId != userId)
            {
                var companyId = await GetCompanyIdByEmployerIdOrThrow(userId);

                await resumeViewRepository.AddResumeView(companyId, resumeId);

                await unitOfWork.SaveChangesAsync();
            }

            return resume.ToDetails();
        }

        public async Task<PagedResponse<ResumeItem>> GetRecommendedResumes(GetRecommendedResumesRequest request)
        {
            var companyId = await GetCompanyIdByEmployerIdOrThrow(request.EmployerId);

            var resumes = await resumeRepository.GetRecommendedResumes(companyId, request.PageIndex, request.PageSize);

            return resumes.ToPagedResponse(request, resume => resume.ToItem());
        }

        public async Task<PagedResponse<ResumeItem>> GetRecommendedResumesForVacancy(GetRecommendedResumesForVacancyRequest request)
        {
            if (!await vacancyRepository.IsEmployerOwnsVacancy(request.EmployerId, request.VacancyId))
                throw new VacancyNotFoundException();

            var resumes = await resumeRepository.GetRecommendedResumesForVacancy(request.VacancyId, request.PageIndex, request.PageSize);

            return resumes.ToPagedResponse(request, resume => resume.ToItem());
        }

        public async Task<PagedResponse<ResumeItem>> SearchResumes(SearchResumeParameters parameters)
        {
            var results = await resumeRepository.SearchResumes(parameters);

            return results.ToPagedResponse(parameters, resume => resume.ToItem());
        }

        public async Task UpdateResume(UpdateResumeRequest request)
        {
            await ThrowIfStudentDoesNotOwnResume(request.StudentId, request.Id);
            
            var skills = request.SkillIds is null
                ? null
                : await TryGetSkillListByIds(request.SkillIds);

            var resume = await resumeRepository.GetResumeForUpdate(request.Id)
                ?? throw new ResumeNotFoundException();

            if (request.SpecializationId.HasValue)
            {
                await ThrowIfSpecializationNotExists(request.SpecializationId.Value);

                resume.SpecializationId = request.SpecializationId.Value;
            }

            if (request.IsActive.HasValue)
                resume.IsActive = request.IsActive.Value;

            if (request.DesiredSalary.HasValue)
                resume.DesiredSalary = request.DesiredSalary.Value;

            if (request.Region is not null)
                resume.Region = StringNormalizer.NormalizeOptional(request.Region);

            if (request.Description is not null)
                resume.Description = StringNormalizer.NormalizeOptional(request.Description);

            if (skills is not null)
            {
                resume.Skills.Clear();

                skills.ForEach(resume.Skills.Add);
            }

            resume.LastUpdateDate = DateOnly.FromDateTime(DateTime.UtcNow);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<int> AddWorkExperience(AddWorkExperienceRequest request)
        {
            await ThrowIfStudentDoesNotOwnResume(request.StudentId, request.ResumeId);

            var we = request.ToDomain();
            await resumeRepository.AddWorkExperience(we);

            await unitOfWork.SaveChangesAsync();

            return we.Id;
        }

        public async Task UpdateWorkExperience(UpdateWorkExperienceRequest request)
        {
            await ThrowIfStudentDoesNotOwnResume(request.StudentId, request.ResumeId);

            await ThrowIfWorkExperienceNotBelongsResume(request.ResumeId, request.Id);

            var workExperience = await resumeRepository.GetWorkExperienceForUpdate(request.Id)
                ?? throw new WorkExperienceNotFoundException();

            if (!string.IsNullOrWhiteSpace(request.CompanyName))
                workExperience.CompanyName = StringNormalizer.NormalizeOptional(request.CompanyName);

            if (!string.IsNullOrWhiteSpace(request.Profession))
                workExperience.Profession = StringNormalizer.NormalizeOptional(request.Profession);

            if (request.StartDateWork.HasValue)
                workExperience.StartDateWork = request.StartDateWork.Value;

            if (request.EndDateWork.HasValue)
                workExperience.EndDateWork = request.EndDateWork;

            if (request.WorkDescription is not null)
                workExperience.WorkDescription = StringNormalizer.NormalizeOptional(request.WorkDescription);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteWorkExperience(int workExperienceId)
        {
            await ThrowIfWorkExperienceNotExists(workExperienceId);

            await resumeRepository.DeleteWorkExperience(workExperienceId);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteResume(int studentId, int resumeId)
        {
            await ThrowIfStudentDoesNotOwnResume(studentId, resumeId);

            await resumeRepository.DeleteResume(resumeId);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task CopyResume(int studentId, int resumeId)
        {
            await ThrowIfStudentDoesNotOwnResume(studentId, resumeId);

            await resumeRepository.CopyResume(resumeId);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
