using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SproutVRSchool.Presentation.ExceptionHandlers;

internal sealed class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // Only handle ValidationException
        // If not, next handler in chain
        if (exception is not Application.Exceptions.ValidationException validationException)
        {
            return false;
        }

        // Set Problem Details response
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "One or more validation errors occurred.",
            Detail = "See the errors property for details.",
            Instance = httpContext.Request.Path,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Extensions =
            {
                ["errors"] = validationException.Errors
                                    .GroupBy(e => e.PropertyName)
                                    .ToDictionary(
                                        g => g.Key,
                                        g => g.Select(e => e.ErrorMessage).ToArray())
            }
        };

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        // exception is handled
        return true;
    }
}
