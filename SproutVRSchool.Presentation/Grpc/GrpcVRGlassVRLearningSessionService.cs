using System.Threading.Channels;
using Grpc.Core;
using LearningSession.V1;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.RoomServices.Publishers;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;
using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession;
using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;
using SproutVRSchool.Application.Extensions;
using SproutVRSchool.Domain;
using SproutVRSchool.Infrastructure.Services.RoomServices;
using StackExchange.Redis;

namespace SproutVRSchool.Presentation.Grpc;

public sealed class GrpcVRGlassVRLearningSessionService : VRGlassSessionManagement.VRGlassSessionManagementBase
{
    // ===============================
    // === Fields
    // ===============================

    private readonly IVRGlassVRLearningSessionService _vrLearningSessionWithVRGlassService;
    private readonly IServerPublishingService _serverPublishingService;
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
        IServerPublishingService serverPublishingService,
        IVRGlassVRLearningSessionService vrLearningSessionWithVRGlassService)
    {
        _vrLearningSessionWithVRGlassService = vrLearningSessionWithVRGlassService;
        _logger = logger;
        _serverPublishingService = serverPublishingService;
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
        string vrLearningSessionId = null;
        string deviceSerialNumber = null;

        // Get the information on the first message, but not skip it since it contains important data.
        Action<ClientToServerMessage> onFirstMessage = (message) =>
        {
            vrLearningSessionId = message.VrLearningSessionId;
            deviceSerialNumber = message.VrDeviceSerialNumber;
            _logger.LogInformation(
                "VR device stream connected. SessionId: {SessionId}, SerialNumber: {SerialNumber}",
                vrLearningSessionId,
                deviceSerialNumber);
        };

        try
        {
            // Listening Background Task and Sending Background Task
            Task listeningTask = ListenForVRDeviceRedisMessagesAsync(requestStream, responseStream, onFirstMessage, context.CancellationToken);
            Task sendingTask = SendServerRedisMessagesToVRDeviceAsync(responseStream, context.CancellationToken);

            // When all tasks complete, log disconnection
            await Task.WhenAll(listeningTask, sendingTask);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Stream failed for SessionId: {SessionId}, SerialNumber: {SerialNumber}", vrLearningSessionId, deviceSerialNumber);
        }
        finally
        {
            // Publish to the Desktop Channel for the disconnected
            if (!string.IsNullOrEmpty(vrLearningSessionId) && !string.IsNullOrEmpty(deviceSerialNumber))
            {
                _logger.LogInformation(
                    "VR device stream disconnected. SessionId: {SessionId}, SerialNumber: {SerialNumber}",
                    vrLearningSessionId,
                    deviceSerialNumber);

                await _serverPublishingService.PublishDeviceDisconnectedAsync(vrLearningSessionId, new DeviceDisconnectedDto()
                {
                    VrDeviceSerialNumber = deviceSerialNumber
                });

                _logger.LogWarning("Invalid VR Learning Session ID or Device Serial Number in the initial message. Disconnecting stream.");
            }
            else
            {
                _logger.LogWarning("VR device stream ended without sending any identifying info.");
            }
        }
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
    private async Task ListenForVRDeviceRedisMessagesAsync(
        IAsyncStreamReader<ClientToServerMessage> requestStream,
        IServerStreamWriter<ServerToClientMessage> responseStream,
        Action<ClientToServerMessage> onFirstMessage,
        CancellationToken cancellationToken)
    {
        bool isFirstMessage = true;

        await foreach (ClientToServerMessage? message in requestStream.ReadAllAsync(cancellationToken))
        {
            // Get the information on the first message
            if (isFirstMessage)
            {
                onFirstMessage(message);
                isFirstMessage = false;
            }

            // Listening messages from VR device
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

                            // Publish Task Updated to the Redis Stream
                            await _vrLearningSessionWithVRGlassService.PublishTaskUpdateToStreamAsync(dto);

                            // when await finish, just fire-and-forget the method and moving on to the next redisMessage
                            _ = responseStream.WriteAsync(
                                ServerToClientMessageFactory.CreateSuccessTaskUpdateConfirmation(message.TaskUpdate.VrTaskId),
                                cancellationToken);

                            break;
                        }

                    case ClientToServerMessage.PayloadOneofCase.None:
                        {
                            _logger.LogWarning("Received redisMessage with no payload from VR device. Session ID: {SessionId}", message.VrLearningSessionId);
                            _ = responseStream.WriteAsync(
                                ServerToClientMessageFactory.CreateUpdateFailedTaskUpdateConfirmation(),
                                cancellationToken);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing redisMessage from client stream.");
                _ = responseStream.WriteAsync(
                    ServerToClientMessageFactory.CreateServerErrorTaskUpdateConfirmation(),
                    cancellationToken);
            }
        }
    }

    private async Task SendServerRedisMessagesToVRDeviceAsync(
        IServerStreamWriter<ServerToClientMessage> responseStream,
        CancellationToken cancellationToken)

    {
        ISubscriber subscriber = _database.Multiplexer.GetSubscriber();
        var channel = Channel.CreateUnbounded<RedisValue>();

        await subscriber.SubscribeAsync(RedisChannel.Literal(AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_NOTIFY_EVENTS_TO_VR_CHANNEL), (redisChannel, message) =>
        {
            // When a redisMessage arrives from Redis, quickly write it to the in-memory queue.
            channel.Writer.TryWrite(message!);
        });

        try
        {
            // Get the redisMessage from in-memory channel
            await foreach (string redisMessage in channel.Reader.ReadAllAsync(cancellationToken))
            {
                _logger.LogInformation("recived redisMessage: {Text}", redisMessage);

                // If redisMessage null, do nothing
                (string? identifier, string? eventType, string? message, bool isEventType) redisEvent = redisMessage.DeparseRedisEventMessage();

                if (!redisEvent.isEventType)
                {
                    continue;
                }

                switch (redisEvent.eventType)
                {
                    case AppCts.Redis.PubSubEvents.END_SIGNAL:
                        {
                            await responseStream.WriteAsync(
                                ServerToClientMessageFactory.CreateEndSessionSignal(_dateTimeProvider.VietNamDateTimeNow),
                                cancellationToken);
                            break;
                        }
                    case AppCts.Redis.PubSubEvents.INFO:
                        {
                            await responseStream.WriteAsync(
                                ServerToClientMessageFactory.CreateInfoNotification(redisEvent.message!),
                                cancellationToken);
                            break;
                        }
                    case AppCts.Redis.PubSubEvents.WARNING:
                        {
                            await responseStream.WriteAsync(
                                ServerToClientMessageFactory.CreateWarningNotification(redisEvent.message!),
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
