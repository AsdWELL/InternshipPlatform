using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.Vacancy;
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
    public class VacancyService(
        IVacancyRepository vacancyRepository,
        IResumeRepository resumeRepository,
        ICompanyRepository companyRepository,
        ISkillRepository skillRepository,
        ISpecializationRepository specializationRepository,
        IFavoriteVacancyService favoriteVacancyService,
        IVacancyViewRepository vacancyViewRepository,
        IUnitOfWork unitOfWork) : IVacancyService
    {
        private async Task<List<Skill>> TryGetSkillListByIds(List<int> skillIds)
        {
            skillIds = skillIds.Distinct().ToList();

            var skills = await skillRepository.GetSkillsByIds(skillIds);

            if (skillIds.Count != skills.Count)
                throw new InvalidResumeSkillsException();

            return skills;
        }

        private async Task<int> TryGetCompanyIdByEmployerId(int employerId)
        {
            var company = await companyRepository.GetCompanyByEmployerId(employerId)
                ?? throw new CompanyNotFoundException();

            return company.Id;
        }
        
        private async Task ThrowIfSpecializationNotExists(int id)
        {
            if (!await specializationRepository.IsSpecializationExists(id))
                throw new InvalidSpecializationException();
        }

        private async Task ThrowIfEmployerDoesNotOwnVacancy(int employerId, int vacancyId)
        {
            if (!await vacancyRepository.IsEmployerOwnsVacancy(employerId, vacancyId))
                throw new VacancyNotFoundException();
        }

        public async Task<int> CreateVacancy(CreateVacancyRequest request)
        {
            var skills = await TryGetSkillListByIds(request.SkillIds);

            await ThrowIfSpecializationNotExists(request.SpecializationId);

            var companyId = await TryGetCompanyIdByEmployerId(request.EmployerId);
            
            var vacancy = request.ToDomain(companyId, skills);
            await vacancyRepository.AddVacancy(vacancy);

            await unitOfWork.SaveChangesAsync();

            return vacancy.Id;
        }

        public async Task<List<VacancyOwnerItem>> GetEmployerVacancies(int employerId)
        {
            var companyId = await TryGetCompanyIdByEmployerId(employerId);
            
            var result = await vacancyRepository.GetCompanyVacancies(companyId);

            return result.Select(v => v.ToOwnerItem()).ToList();
        }

        public async Task<List<VacancyItem>> GetCompanyVacancies(int userId, int companyId)
        {
            var result = await vacancyRepository.GetCompanyVacancies(companyId);

            return await favoriteVacancyService.MapToItemAndMarkFavorites(userId, result.Select(r => r.Vacancy));
        }

        public async Task<VacancyDetails> GetVacancyDetailsForStudent(int studentId, int vacancyId)
        {
            var vacancy = await vacancyRepository.GetVacancyById(vacancyId)
                ?? throw new VacancyNotFoundException();

            if (!vacancy.IsActive)
                throw new VacancyNotFoundException();

            await vacancyViewRepository.AddVacancyView(studentId, vacancyId);
            await unitOfWork.SaveChangesAsync();

            var isFavorite = await favoriteVacancyService.IsVacancyInFavorites(studentId, vacancyId);

            return vacancy.ToDetails(isFavorite);
        }

        public async Task<VacancyOwnerDetails> GetVacancyDetailsForOwner(int employerId, int vacancyId)
        {
            await ThrowIfEmployerDoesNotOwnVacancy(employerId, vacancyId);

            var vacancy = await vacancyRepository.GetVacancyById(vacancyId)
                ?? throw new VacancyNotFoundException();

            return vacancy.ToOwnerDetails();
        }

        public async Task<PagedResponse<VacancyItem>> GetRecommendedVacancies(GetRecommendedVacanciesRequest request)
        {
            var vacancies = await vacancyRepository.GetRecommendedVacancies(
                request.StudentId, request.PageIndex, request.PageSize);

            var items = await favoriteVacancyService.MapToItemAndMarkFavorites(request.StudentId, vacancies.Items);

            return vacancies.ToPagedResponse(request, items);
        }

        public async Task<PagedResponse<VacancyItem>> GetRecommendedVacanciesForResume(GetRecommendedVacanciesForResumeRequest request)
        {
            if (!await resumeRepository.IsStudentOwnsResume(request.StudentId, request.ResumeId))
                throw new ResumeNotFoundException();

            var vacancies = await vacancyRepository.GetRecommendedVacanciesForResume(
                request.ResumeId, request.PageIndex, request.PageSize);

            var items = await favoriteVacancyService.MapToItemAndMarkFavorites(request.StudentId, vacancies.Items);

            return vacancies.ToPagedResponse(request, items);
        }

        public async Task<PagedResponse<VacancyItem>> SearchVacancies(SearchVacancyParameters parameters)
        {
            var result = await vacancyRepository.SearchVacancies(parameters);

            var items = await favoriteVacancyService.MapToItemAndMarkFavorites(parameters.StudentId, result.Items);

            return result.ToPagedResponse(parameters, items);
        }

        public async Task UpdateVacancy(UpdateVacancyRequest request)
        {
            await ThrowIfEmployerDoesNotOwnVacancy(request.EmployerId, request.Id);

            var skills = request.SkillIds is null
                ? null
                : await TryGetSkillListByIds(request.SkillIds);

            var vacancy = await vacancyRepository.GetVacancyForUpdate(request.Id)
                ?? throw new VacancyNotFoundException();

            if (request.SpecializationId is not null)
            {
                await ThrowIfSpecializationNotExists(request.SpecializationId.Value);

                vacancy.SpecializationId = request.SpecializationId.Value;
            }

            if (!string.IsNullOrWhiteSpace(request.Title))
                vacancy.Title = StringNormalizer.NormalizeRequired(request.Title);

            if (request.Description is not null)
                vacancy.Description = StringNormalizer.NormalizeOptional(request.Description);

            if (request.SalaryFrom is not null)
                vacancy.SalaryFrom = request.SalaryFrom.Value;

            if (request.SalaryTo is not null)
                vacancy.SalaryTo = request.SalaryTo.Value;

            if (request.IsRemote is not null)
                vacancy.IsRemote = request.IsRemote.Value;

            if (request.Region is not null)
                vacancy.Region = StringNormalizer.NormalizeOptional(request.Region);

            if (request.IsActive is not null)
                vacancy.IsActive = request.IsActive.Value;

            if (request.MinWorkExperienceYears is not null)
                vacancy.MinWorkExperienceYears = request.MinWorkExperienceYears.Value;

            if (skills is not null)
            {
                vacancy.Skills.Clear();

                skills.ForEach(vacancy.Skills.Add);
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteVacancy(int employerId, int vacancyId)
        {
            await ThrowIfEmployerDoesNotOwnVacancy(employerId, vacancyId);

            await vacancyRepository.DeleteVacancy(vacancyId);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
