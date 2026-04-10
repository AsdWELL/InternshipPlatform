using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.Vacancy;
using InternshipPlatform.Application.Exceptions.Company;
using InternshipPlatform.Application.Exceptions.Specialization;
using InternshipPlatform.Application.Exceptions.Vacancy;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;
using InternshipPlatform.Application.Utils;

namespace InternshipPlatform.Application.Services
{
    public class VacancyService(
        IVacancyRepository vacancyRepository,
        ICompanyRepository companyRepository,
        ISpecializationRepository specializationRepository,
        IUnitOfWork unitOfWork) : IVacancyService
    {
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
            await ThrowIfSpecializationNotExists(request.SpecializationId);

            var companyId = await TryGetCompanyIdByEmployerId(request.EmployerId);
            
            var vacancy = request.ToDomain(companyId);
            await vacancyRepository.AddVacancy(vacancy);

            await unitOfWork.SaveChangesAsync();

            return vacancy.Id;
        }

        public async Task<List<VacancyItem>> GetEmployerVacancies(int employerId)
        {
            var companyId = await TryGetCompanyIdByEmployerId(employerId);
            
            var vacancies = await vacancyRepository.GetCompanyVacancies(companyId);

            return vacancies.Select(v => v.ToItem()).ToList();
        }

        public async Task<VacancyDetails> GetVacancyDetails(int vacancyId)
        {
            var vacancy = await vacancyRepository.GetVacancyById(vacancyId)
                ?? throw new VacancyNotFoundException();

            return vacancy.ToDetails();
        }

        public async Task<PagedResponse<VacancyItem>> SearchVacancies(SearchVacancyParameters parameters)
        {
            var result = await vacancyRepository.SearchVacancies(parameters);

            return result.ToPagedResponse(parameters, vacancy => vacancy.ToItem());
        }

        public async Task UpdateVacancy(UpdateVacancyRequest request)
        {
            await ThrowIfEmployerDoesNotOwnVacancy(request.EmployerId, request.Id);

            var vacancy = await vacancyRepository.GetVacancyForUpdate(request.Id)
                ?? throw new VacancyNotFoundException();

            if (request.SpecializationId is not null)
            {
                await ThrowIfSpecializationNotExists(request.SpecializationId.Value);

                vacancy.SpecializationId = request.SpecializationId.Value;
            }

            if (request.Title is not null)
                vacancy.Title = StringNormalizer.NormalizeRequired(request.Title);

            if (request.Description is not null)
                vacancy.Description = StringNormalizer.NormalizeOptional(request.Description);

            if (request.SalaryFrom is not null)
                vacancy.SalaryFrom = request.SalaryFrom;

            if (request.SalaryTo is not null)
                vacancy.SalaryTo = request.SalaryTo;

            if (request.IsRemote is not null)
                vacancy.IsRemote = request.IsRemote.Value;

            if (request.Region is not null)
                vacancy.Region = StringNormalizer.NormalizeOptional(request.Region);

            if (request.IsActive is not null)
                vacancy.IsActive = request.IsActive.Value;

            if (request.MinWorkExperienceYears is not null)
                vacancy.MinWorkExperienceYears = request.MinWorkExperienceYears.Value;

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
