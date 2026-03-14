using InternshipPlatform.Application.Exceptions.Base;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InternshipPlatform.Api.Filters
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception;
            var httpContext = context.HttpContext;

            httpContext.Response.StatusCode = exception switch
            {
                UnathorizedException => StatusCodes.Status401Unauthorized,
                NotFoundException => StatusCodes.Status404NotFound,
                ConflictException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            httpContext.Response.WriteAsJsonAsync(new
            {
                ActionName = context.ActionDescriptor.DisplayName,
                exception.Message,
                exception.StackTrace
            });

            context.ExceptionHandled = true;

            return Task.CompletedTask;
        }
    }
}
