using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Abstractions.Clock;

namespace SproutVRSchool.Application.Behaviors;

internal sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> _logger,
    IDateTimeProvider dateTimeProvider
    ) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    // ============================
    // === Methods
    // ============================

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Before handling 
        string requestName = typeof(TRequest).Name;

        _logger.LogInformation(
            """
                Starting MediatR Request: {RequestName}
                - VN Time:     {DatetimeVn}
                - UTC Time:    {DatetimeUtc}
                - Request Body: {RequestBody}
                """,
            requestName,
            dateTimeProvider.VietNamDateTimeNow,
            dateTimeProvider.UtcDateTimeNow,
            JsonSerializer.Serialize(request));

        var stopwatch = Stopwatch.StartNew();

        // Proceed to the next behavior or handler
        TResponse? response = await next(cancellationToken);

        // After handling
        stopwatch.Stop();
        _logger.LogInformation(
            "Finished MediatR Request: {RequestName} in {ElapsedMilliseconds}ms",
            requestName,
            stopwatch.ElapsedMilliseconds);

        return response;
    }
}
