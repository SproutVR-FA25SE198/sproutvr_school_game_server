using Grpc.Core;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Exceptions.Resources;

namespace SproutVRSchool.Presentation.ExceptionHandlers;

internal sealed class ResourcesExceptionHandler(
    ILogger<ResourcesExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails = null;

        switch (exception)
        {
            // 404 - Resource not found
            case SvrResourceNotFoundException notFoundException:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "The requested resource was not found.",
                    Detail = notFoundException.Message,
                    Instance = httpContext.Request.Path,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
                };

                break;

            // 400 - Errors related to resource validation
            case SvrResourceValidationException validationException:
                problemDetails = new ProblemDetails
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
                break;

            default:
                return false;
        }

        logger.LogWarning("Resources exception handled: {Title} - {Message}", problemDetails.Title, exception.Message);

        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
