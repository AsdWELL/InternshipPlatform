using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.Resume;
using InternshipPlatform.Application.Exceptions.Resume;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Services
{
    public class ResumeService(
        IResumeRepository resumeRepository,
        ISkillRepository skillRepository,
        ISpecializationRepository specializationRepository,
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

        public async Task<int> CreateResume(CreateResumeRequest request)
        {
            var skills = await TryGetSkillListByIds(request.SkillIds);            

            await ThrowIfSpecializationNotExists(request.SpecializationId);

            var resume = request.ToDomain(skills);
            await resumeRepository.AddResume(resume);

            await unitOfWork.SaveChangesAsync();

            return resume.Id;
        }

        public async Task<List<ResumeItem>> GetStudentResumes(int studentId)
        {
            var resumes = await resumeRepository.GetStudentResumes(studentId);

            return resumes.Select(r => r.ToItem()).ToList();
        }

        public async Task<ResumeDetails> GetResumeDetails(int resumeId)
        {
            var resume = await resumeRepository.GetResumeById(resumeId)
                ?? throw new ResumeNotFoundException();

            return resume.ToDetails();
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

            await resumeRepository.UpdateResume(request.ToDomain(skills));

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

            await ThrowIfWorkExperienceNotExists(request.Id);

            await ThrowIfWorkExperienceNotBelongsResume(request.ResumeId, request.Id);

            await resumeRepository.UpdateWorkExperience(request.ToDomain());

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
