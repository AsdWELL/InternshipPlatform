using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.Chat;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.Chats
{
    [Authorize(Roles = Roles.Employer)]
    [Route("api/employers/me/chats")]
    public class EmployerChatsController(
        IChatService chatService,
        IMessageService messageService) : AuthorizedUserController
    {
        [HttpGet]
        public async Task<IActionResult> GetEmployerChats()
        {
            return Ok(await chatService.GetEmployerChats(UserId));
        }

        [HttpPost]
        public async Task<IActionResult> GetOrCreateEmployerChat([FromBody] StartEmployerChatRequest request)
        {
            request.EmployerId = UserId;

            return Ok(await chatService.GetOrCreateEmployerChat(request));
        }

        [HttpGet("{chatId:int}")]
        public async Task<IActionResult> GetEmployerChatMessages([FromRoute] int chatId)
        {
            return Ok(await chatService.GetEmployerChatMessages(UserId, chatId));
        }

        [HttpPost("{chatId:int}/read")]
        public async Task<IActionResult> MarkEmployerChatAsRead([FromRoute] int chatId)
        {
            await messageService.MarkEmployerChatAsRead(UserId, chatId);

            return Ok();
        }
    }
}
