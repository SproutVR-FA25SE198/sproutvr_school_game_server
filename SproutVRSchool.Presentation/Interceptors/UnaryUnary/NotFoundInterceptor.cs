using Grpc.Core;
using Grpc.Core.Interceptors;
using SproutVRSchool.Application.Exceptions;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Interceptors.UnaryUnary;

public sealed class NotFoundInterceptor(
    ILogger<NotFoundInterceptor> logger)
    : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (SvrNotFoundException ex)
        {
            // warining for development
            logger.LogDebug(ex, "Not Found Exception intercepted: {Message}", ex.Message);

            var metadata = new Metadata
            {
                { AppCts.Grpc.ERROR_MESSAGE_KEY, ex.Message }
            };

            throw new RpcException(new Status(StatusCode.NotFound, ex.Message), metadata);
        }
    }
}
