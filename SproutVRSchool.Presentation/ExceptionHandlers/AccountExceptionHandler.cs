using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Exceptions.Accounts;

namespace SproutVRSchool.Presentation.ExceptionHandlers;

internal sealed class AccountExceptionHandler(ILogger<AccountExceptionHandler> logger)
 : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ProblemDetails? problemDetails = null;

        // 1. Check the account-exception types
        switch (exception)
        {
            // 401 - Bad Credentials
            case SvrUnauthorizedAccessException unauthorizedException:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Unauthorized Access",
                    Detail = unauthorizedException.Message,
                    Instance = httpContext.Request.Path,
                    Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
                };
                break;

            // 403 - Forbidden
            case SvrForbiddenAccessException forbiddenExeption:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status403Forbidden,
                    Title = "Access Denied",
                    Detail = forbiddenExeption.Message,
                    Instance = httpContext.Request.Path,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
                };
                break;

            // 400 - OTP Failure
            case SvrOtpFailureException otpFailureException:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "OTP Verification Failed",
                    Detail = otpFailureException.Message,
                    Instance = httpContext.Request.Path,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                };
                break;

            // 400 - Invalid Account Type
            case SvrInvalidAccountTypeStatusChangeException invalidAccountTypeException:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Account Type",
                    Detail = invalidAccountTypeException.Message,
                    Instance = httpContext.Request.Path,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                };
                break;

            // 403 - Selft Status Change
            case SvrSelfAccountStatusChangeException selfStatusChangeException:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status403Forbidden,
                    Title = "Operation Not Allowed",
                    Detail = selfStatusChangeException.Message,
                    Instance = httpContext.Request.Path,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
                };
                break;

            default:
                return false;
        }

        logger.LogWarning("Account exception handled: {Title} - {Message}", problemDetails.Title, exception.Message);

        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
