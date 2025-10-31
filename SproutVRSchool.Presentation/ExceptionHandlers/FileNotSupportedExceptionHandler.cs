using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Exceptions;

namespace SproutVRSchool.Presentation.ExceptionHandlers;

public class FileNotSupportedExceptionHandler(
    ILogger<FileNotSupportedExceptionHandler> _logger) : IExceptionHandler
{

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // If not, move to the next handler in the pipeline
        if (exception is not SvrFileNotSupportedException fileException)
        {
            return false;
        }

        _logger.LogWarning("File not supported: {Message}", fileException.Message);

        var problemDetails = new ProblemDetails
        {
            Title = "File Not Supported",
            Status = StatusCodes.Status415UnsupportedMediaType,
            Detail = fileException.Message,
            Extensions = {
                ["fileName"] = fileException.FileName
            },
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.13"
        };

        httpContext.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        // exception is handled, return problem details to client
        return true;
    }
}
