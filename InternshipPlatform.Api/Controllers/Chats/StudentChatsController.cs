using InternshipPlatform.Api.Authorization;
using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.Chat;
using InternshipPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.Chats
{
    [Authorize(Policy = Policies.StudentMustHaveGroup)]
    [Route("api/students/me/chats")]
    public class StudentChatsController(
        IChatService chatService,
        IMessageService messageService) : AuthorizedUserController
    {
        [HttpGet]
        public async Task<IActionResult> GetStudentChats()
        {
            return Ok(await chatService.GetStudentChats(UserId));
        }

        [HttpPost]
        public async Task<IActionResult> GetOrCreateStudentChat([FromBody] StartStudentChatRequest request)
        {
            request.StudentId = UserId;
            
            return Ok(await chatService.GetOrCreateStudentChat(request));
        }

        [HttpGet("{chatId:int}")]
        public async Task<IActionResult> GetStudentChatMessages([FromRoute] int chatId)
        {
            return Ok(await chatService.GetStudentChatMessages(UserId, chatId));
        }

        [HttpPost("{chatId:int}/read")]
        public async Task<IActionResult> MarkStudentChatAsRead([FromRoute] int chatId)
        {
            await messageService.MarkStudentChatAsRead(UserId, chatId);

            return Ok();
        }
    }
}
