using System.Threading.Channels;
using Grpc.Core;
using LearningSession.V1;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession;
using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;
using SproutVRSchool.Domain;
using SproutVRSchool.Infrastructure.Services.RoomServices;
using StackExchange.Redis;

namespace SproutVRSchool.Presentation.Grpc;

public sealed class GrpcVRGlassVRLearningSessionService : VRGlassSessionManagement.VRGlassSessionManagementBase
{
    // ===============================
    // === Fields
    // ===============================

    private readonly IVRLearningSessionWithVRGlassService _vrLearningSessionWithVRGlassService;
    private readonly IDatabase _database;
    private readonly ILogger<GrpcVRGlassVRLearningSessionService> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;

    // ==============================
    // === Constructors
    // ==============================

    public GrpcVRGlassVRLearningSessionService(
        ILogger<GrpcVRGlassVRLearningSessionService> logger,
        IConnectionMultiplexer connectionMultiplexer,
        IDateTimeProvider dateTimeProvider,
        IVRLearningSessionWithVRGlassService vrLearningSessionWithVRGlassService)
    {
        _vrLearningSessionWithVRGlassService = vrLearningSessionWithVRGlassService;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
        _database = connectionMultiplexer.GetDatabase();
    }

    // ==============================
    // === Methods
    // ==============================

    public override async Task<JoinRoomResponse> JoinRoom(JoinRoomRequest request, ServerCallContext context)
    {
        var joinRoomRequestDto = JoinRoomRequestDto.MapFromGrpcRequest(request);
        JoinRoomResponseDto resultDto = await _vrLearningSessionWithVRGlassService.JoinRoomAsync(joinRoomRequestDto);
        return JoinRoomResponseDto.MapToGrpcResponse(resultDto);
    }

    public override async Task StreamSessionState(
        IAsyncStreamReader<ClientToServerMessage> requestStream, IServerStreamWriter<ServerToClientMessage> responseStream, ServerCallContext context)
    {
        // Listening Background Task and Sending Background Task
        Task listeningTask = ListenForClientMessagesAsync(requestStream, responseStream, context.CancellationToken);
        Task sendingTask = SendServerMessagesAsync(requestStream, responseStream, context.CancellationToken);

        await Task.WhenAll(listeningTask, sendingTask);
        _logger.LogInformation("VR device stream disconnected for VR Learning Session ID: {SessionId}", requestStream.Current.VrLearningSessionId);
    }

    // =================================
    // === Helper Methods for Streaming
    // =================================

    /// <summary>
    /// This task is dedicated to LISTENING for messages FROM the client.
    /// </summary>
    /// <param name="requestStream"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task ListenForClientMessagesAsync(
        IAsyncStreamReader<ClientToServerMessage> requestStream,
        IServerStreamWriter<ServerToClientMessage> responseStream,
        CancellationToken cancellationToken)
    {
        await foreach (ClientToServerMessage? message in requestStream.ReadAllAsync(cancellationToken))
        {
            try
            {
                switch (message.PayloadCase)
                {
                    case ClientToServerMessage.PayloadOneofCase.TaskUpdate:
                        {
                            _logger.LogInformation("Received TaskUpdate from VR device. Session ID: {SessionId}", message.VrLearningSessionId);
                            var dto = PublishTaskUpdateRequestDto.MapFromGrpcRequest(
                                message.TaskUpdate,
                                message.VrLearningSessionId,
                                message.VrDeviceSerialNumber);

                            // await to make sure Task Update in order
                            await _vrLearningSessionWithVRGlassService.PublishTaskUpdateToStreamAsync(dto);

                            // when await finish, just fire-and-forget the method and moving on to the next message
                            _ = responseStream.WriteAsync(
                                ServerToClientMessageFactory.CreateSuccessTaskUpdateConfirmation(message.TaskUpdate.VrTaskId),
                                cancellationToken);

                            break;
                        }

                    case ClientToServerMessage.PayloadOneofCase.None:
                        {
                            _logger.LogWarning("Received message with no payload from VR device. Session ID: {SessionId}", message.VrLearningSessionId);
                            _ = responseStream.WriteAsync(
                                ServerToClientMessageFactory.CreateUpdateFailedTaskUpdateConfirmation(),
                                cancellationToken);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message from client stream.");
                _ = responseStream.WriteAsync(
                    ServerToClientMessageFactory.CreateServerErrorTaskUpdateConfirmation(),
                    cancellationToken);
            }
        }
    }

#pragma warning disable S1172 // Unused method parameters should be removed
    private async Task SendServerMessagesAsync(
        IAsyncStreamReader<ClientToServerMessage> requestStream,
        IServerStreamWriter<ServerToClientMessage> responseStream,
        CancellationToken cancellationToken)

    {
        ISubscriber subscriber = _database.Multiplexer.GetSubscriber();
        var channel = Channel.CreateUnbounded<RedisValue>();

        await subscriber.SubscribeAsync(RedisChannel.Literal(AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_NOTIFY_EVENTS), (redisChannel, message) =>
        {
            // When a message arrives from Redis, quickly write it to the in-memory queue.
            channel.Writer.TryWrite(message!);
        });

        try
        {
            // Get the message from in-memory channel
            await foreach (string message in channel.Reader.ReadAllAsync(cancellationToken))
            {
                _logger.LogInformation("recived message: {Text}", message);

                // If message null, do nothing
                if (string.IsNullOrEmpty(message))
                {
                    continue;
                }

                // if not having vrLearningSessionId:eventType:text, continue
                string[] parts = message.ToString().Split(":", 3);
                if (parts.Length < 2)
                {
                    continue;
                }

                // UNDONE: Testing purpose
                string eventType = parts[1]!.ToUpper(System.Globalization.CultureInfo.CurrentCulture);
                string text = parts[2];

                switch (eventType)
                {
                    case "ENDSIGNAL":
                        {
                            await responseStream.WriteAsync(
                                ServerToClientMessageFactory.CreateEndSessionSignal(_dateTimeProvider.VietNamDateTimeNow),
                                cancellationToken);
                            break;
                        }
                    case "INFO":
                        {
                            await responseStream.WriteAsync(
                                ServerToClientMessageFactory.CreateInfoNotification(text),
                                cancellationToken);
                            break;
                        }
                    case "WARNING":
                        {
                            await responseStream.WriteAsync(
                                ServerToClientMessageFactory.CreateWarningNotification(text),
                                cancellationToken);
                            break;
                        }
                }
            }
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation(ex, "VR device stream disconnected for VR Learning Session ID");
        }
    }
}
