using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SproutVRSchool.Presentation.ExceptionHandlers;

internal sealed class GeneralExceptionHandler(
    ILogger<GeneralExceptionHandler> _logger

    ) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred.",
            Detail = "Please try again later or contact support if the problem persists.",
            Instance = httpContext.Request.Path,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        // exception is handled
        return true;
    }
}
