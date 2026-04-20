using InternshipPlatform.Application.Dtos.Chat;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class ChatRepository(InternshipPlatformContext context) : IChatRepository
    {
        public async Task<bool> CanStudentAccessChat(int studentId, int chatId)
        {
            return await context.Chats
                .AsNoTracking()
                .AnyAsync(c => c.Id == chatId && c.StudentId == studentId);
        }

        public async Task<bool> CanEmployerAccessChat(int employerId, int chatId)
        {
            return await context.Chats
                .AsNoTracking()
                .AnyAsync(c => c.Id == chatId 
                    && c.CompanyId == context.EmployerProfiles
                        .Where(ep => ep.UserId == employerId)
                        .Select(ep => ep.CompanyId)
                        .FirstOrDefault());
        }

        public async Task<bool> CanStudentSendToChat(int studentId, int chatId)
        {
            return await context.Chats
                .AsNoTracking()
                .AnyAsync(c => c.Id == chatId 
                    && c.StudentId == studentId
                    && !c.IsClosed);
        }

        public async Task<bool> CanEmployerSendToChat(int employerId, int chatId)
        {
            return await context.Chats
                .AsNoTracking()
                .AnyAsync(c => c.Id == chatId
                    && !c.IsClosed
                    && c.CompanyId == context.EmployerProfiles
                        .Where(ep => ep.UserId == employerId)
                        .Select(ep => ep.CompanyId)
                        .FirstOrDefault());
        }

        public async Task AddChat(Chat chat)
        {
            await context.Chats.AddAsync(chat);
        }

        public async Task<Chat?> GetChatWithMessagesById(int chatId)
        {
            return await context.Chats
                .AsNoTracking()
                .Include(c => c.StudentProfile)
                .Include(c => c.Vacancy)
                .Include(c => c.Company)
                .Include(c => c.Messages)
                .Include(c => c.Application)
                    .ThenInclude(a => a.Resume)
                        .ThenInclude(r => r.Specialization)
                .FirstOrDefaultAsync(c => c.Id == chatId);
        }

        public async Task<Chat?> GetActiveChatByStudentAndVacancy(int studentId, int vacancyId)
        {
            return await context.Chats
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.StudentId == studentId 
                    && c.VacancyId == vacancyId
                    && !c.IsClosed);
        }

        public async Task<Chat?> GetChatByApplicationIdForUpdate(int applicationId)
        {
            return await context.Chats
                .FirstOrDefaultAsync(c => c.Application.Id == applicationId);
        }

        public async Task<List<StudentChatResult>> GetStudentChats(int studentId)
        {            
            return await context.Chats
                .AsNoTracking()
                .Where(c => c.StudentId == studentId)
                .Select(c =>
                    new StudentChatResult
                    {
                        Id = c.Id,
                        CompanyId = c.CompanyId,
                        CompanyName = c.Company.Name,
                        CompanyLogoPath = c.Company.LogoPath,
                        VacancyId = c.VacancyId,
                        VacancyTitle = c.Vacancy.Title,
                        LastMessage = context.Messages
                            .Where(m => m.ChatId == c.Id)
                            .OrderByDescending(m => m.CreatedAt)
                            .FirstOrDefault(),
                        UnreadMessagesCount = context.Messages
                            .Where(m => m.ChatId == c.Id 
                                && m.SenderUserId != studentId
                                && !m.IsRead)
                            .Count(),
                        IsClosed = c.IsClosed
                    }
                )
                .OrderByDescending(c => c.LastMessage.CreatedAt)
                    .ThenByDescending(c => c.Id)
                .ToListAsync();
        }

        public async Task<List<EmployerChatResult>> GetEmployerChats(int employerId)
        {
            return await context.Chats
                .AsNoTracking()
                .Where(c => c.CompanyId == context.EmployerProfiles
                        .Where(ep => ep.UserId == employerId)
                        .Select(ep => ep.CompanyId)
                        .FirstOrDefault())
                .Select(c => new EmployerChatResult
                {
                    Id = c.Id,
                    StudentId = c.StudentId,
                    StudentName = c.StudentProfile.Name,
                    StudentSurname = c.StudentProfile.Surname,
                    StudentPatronymic = c.StudentProfile.Patronymic,
                    StudentAvatarPath = c.StudentProfile.AvatarPath,
                    SpecializationName = c.Application == null
                        ? null
                        : c.Application.Resume.Specialization.Name,
                    VacancyId = c.VacancyId,
                    VacancyTitle = c.Vacancy.Title,
                    LastMessage = context.Messages
                            .Where(m => m.ChatId == c.Id)
                            .OrderByDescending(m => m.CreatedAt)
                            .FirstOrDefault(),
                    UnreadMessagesCount = context.Messages
                            .Where(m => m.ChatId == c.Id
                                && m.SenderUserId != employerId
                                && !m.IsRead)
                            .Count(),
                    IsClosed = c.IsClosed
                })
                .ToListAsync();
        }
    }
}
