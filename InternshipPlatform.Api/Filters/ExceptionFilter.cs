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
                BadRequestException => StatusCodes.Status400BadRequest,
                UnathorizedException => StatusCodes.Status401Unauthorized,
                NotFoundException => StatusCodes.Status404NotFound,
                ConflictException => StatusCodes.Status409Conflict,
                UnsupportedMediaTypeException => StatusCodes.Status415UnsupportedMediaType,
                _ => StatusCodes.Status500InternalServerError
            };

            if (exception is ConflictException validationException)
                httpContext.Response.WriteAsJsonAsync(new
                {
                    Title = "One or more validation errors occurred.",
                    Errors = new Dictionary<string, string>
                    {
                        [validationException.PropertyName] = validationException.Message
                    }
                });
            else
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
