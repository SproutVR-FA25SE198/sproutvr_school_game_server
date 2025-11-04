using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Exceptions.ContentSeedings;

namespace SproutVRSchool.Presentation.ExceptionHandlers;

public class ContentSeedingsExceptionHandler(
    ILogger<ContentSeedingsExceptionHandler> logger) : IExceptionHandler
{

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ProblemDetails? problemDetails = null;

        switch (exception)
        {
            // 415 Unsupported Media Type
            case SvrFileNotSupportedException fileEx:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status415UnsupportedMediaType,
                    Title = "File Not Supported",
                    Detail = fileEx.Message,
                    Instance = httpContext.Request.Path,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.13",
                    Extensions = { ["fileName"] = fileEx.FileName }
                };
                break;

            // 409 Conflict
            case SvrAlreadyInstalledException installedEx:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Already Installed",
                    Detail = installedEx.Message,
                    Instance = httpContext.Request.Path,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8"
                };
                break;

            // 424 Failed Dependency
            case SvrDownloadFailedException downloadEx:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status424FailedDependency,
                    Title = "Download Failed",
                    Detail = downloadEx.Message,
                    Instance = httpContext.Request.Path,
                    Type = "https://datatracker.ietf.org/doc/html/rfc4918#section-11.4"
                };
                break;

            // 500 Internal Server Error
            case SvrInstallFailedException installEx:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Installation Failed",
                    Detail = installEx.Message,
                    Instance = httpContext.Request.Path,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
                };
                break;

            default:
                return false;
        }

        logger.LogWarning("Content seeding exception handled: {Title} - {Message}", problemDetails.Title, exception.Message);

        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
