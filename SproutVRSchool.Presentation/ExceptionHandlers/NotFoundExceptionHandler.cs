using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Exceptions;

namespace SproutVRSchool.Presentation.ExceptionHandlers;

internal sealed class NotFoundExceptionHandler(
    ILogger<NotFoundExceptionHandler> _logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException notFoundException)
        {
            return false;
        }

        _logger.LogWarning("Resource not found: {Message}", notFoundException.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "The requested resource was not found.",
            Detail = notFoundException.Message,
            Instance = httpContext.Request.Path,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        // Exception is handled
        return true;
    }
}
