using InternshipPlatform.Application.Dtos.Chat;
using InternshipPlatform.Application.Exceptions.Chat;
using InternshipPlatform.Application.Exceptions.Company;
using InternshipPlatform.Application.Exceptions.StudentProfile;
using InternshipPlatform.Application.Exceptions.Vacancy;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;
using InternshipPlatform.Application.Values;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Services
{
    public class ChatService(
        IChatRepository chatRepository,
        IMessageRepository messageRepository,
        IVacancyRepository vacancyRepository,
        IStudentProfileRepository studentProfileRepository,
        ICompanyRepository companyRepository,
        IUnitOfWork unitOfWork) : IChatService
    {
        private async Task<Vacancy> GetActiveVacancyOrThrow(int vacancyId)
        {
            var vacancy = await vacancyRepository.GetVacancyById(vacancyId)
                ?? throw new VacancyNotFoundException();

            if (!vacancy.IsActive)
                throw new VacancyNotFoundException();

            return vacancy;
        }

        public async Task ThrowIfUserCantAccessChat(int userId, string role, int chatId)
        {
            var accessed = role switch
            {
                Roles.Student => await chatRepository.CanStudentAccessChat(userId, chatId),
                Roles.Employer => await chatRepository.CanEmployerAccessChat(userId, chatId),
                _ => false
            };

            if (!accessed)
                throw new ChatNotFoundException();
        }

        public async Task<int> GetOrCreateStudentChat(StartStudentChatRequest request)
        {
            var vacancy = await GetActiveVacancyOrThrow(request.VacancyId);

            var chat = await chatRepository.GetActiveChatByStudentAndVacancy(request.StudentId, request.VacancyId);

            if (chat is null)
            {
                chat = request.ToDomain(vacancy.CompanyId);
                
                await chatRepository.AddChat(chat);
                await unitOfWork.SaveChangesAsync();
            }

            if (!string.IsNullOrWhiteSpace(request.Message))
            {
                await messageRepository.AddMessage(new Message
                {
                    ChatId = chat.Id,
                    Content = request.Message,
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false,
                    SenderUserId = request.StudentId
                });
                await unitOfWork.SaveChangesAsync();
            }

            return chat.Id;
        }

        public async Task<int> GetOrCreateEmployerChat(StartEmployerChatRequest request)
        {
            if (!await studentProfileRepository.IsStudentExists(request.StudentId))
                throw new StudentProfileNotFoundException();

            if (!await vacancyRepository.IsEmployerOwnsVacancy(request.EmployerId, request.VacancyId))
                throw new VacancyNotFoundException();

            var vacancy = await GetActiveVacancyOrThrow(request.VacancyId);

            var chat = await chatRepository.GetActiveChatByStudentAndVacancy(request.StudentId, request.VacancyId);

            if (chat is null)
            {
                chat = request.ToDomain(vacancy.CompanyId);

                await chatRepository.AddChat(chat);
                await unitOfWork.SaveChangesAsync();
            }

            if (!string.IsNullOrWhiteSpace(request.Message))
            {
                await messageRepository.AddMessage(new Message
                {
                    ChatId = chat.Id,
                    Content = request.Message,
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false,
                    SenderUserId = request.EmployerId
                });
                await unitOfWork.SaveChangesAsync();
            }

            return chat.Id;
        }

        public async Task<List<StudentChatItem>> GetStudentChats(int studentId)
        {
            var chats = await chatRepository.GetStudentChats(studentId);

            return chats.Select(c => c.ToItem())
                .OrderByDescending(c => c.LastMessage.CreatedAt)
                .ToList();
        }

        public async Task<List<EmployerChatItem>> GetEmployerChats(int employerId)
        {
            var chats = await chatRepository.GetEmployerChats(employerId);

            return chats.Select(c => c.ToItem())
                .OrderByDescending(c => c.LastMessage?.CreatedAt)
                .ToList();
        }

        public async Task<StudentChatMessagesResponse> GetStudentChatMessages(int studentId, int chatId)
        {
            var chat = await chatRepository.GetChatWithMessagesById(chatId)
                ?? throw new ChatNotFoundException();

            if (chat.StudentId != studentId)
                throw new ChatNotFoundException();

            return chat.ToStudentResponse();
        }

        public async Task<EmployerChatMessagesResponse> GetEmployerChatMessages(int employerId, int chatId)
        {
            var chat = await chatRepository.GetChatWithMessagesById(chatId)
                ?? throw new ChatNotFoundException();

            var company = await companyRepository.GetCompanyByEmployerId(employerId)
                ?? throw new CompanyNotFoundException();

            if (chat.CompanyId != company.Id)
                throw new ChatNotFoundException();

            return chat.ToEmployerResponse();
        }

        public async Task CloseChatByApplicationId(int userId, string role, int applicationId)
        {
            var chat = await chatRepository.GetChatByApplicationIdForUpdate(applicationId)
                ?? throw new ChatNotFoundException();

            await ThrowIfUserCantAccessChat(userId, role, chat.Id);

            chat.IsClosed = true;
        }
    }
}
