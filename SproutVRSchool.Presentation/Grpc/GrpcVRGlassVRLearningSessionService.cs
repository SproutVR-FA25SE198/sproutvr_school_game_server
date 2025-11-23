using System.Threading.Channels;
using Grpc.Core;
using LearningSession.V1;
using SproutVRSchool.Application.Abstractions.Clock;
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

    private readonly IVRGlassVRLearningSessionService _vrGlassVRLearningSessionService;
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
        IVRGlassVRLearningSessionService vrGlassVRLearningSessionService)
    {
        _vrGlassVRLearningSessionService = vrGlassVRLearningSessionService;
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
        JoinRoomResponseDto resultDto = await _vrGlassVRLearningSessionService.JoinRoomAsync(joinRoomRequestDto);
        return JoinRoomResponseDto.MapToGrpcResponse(resultDto);
    }

    public override async Task StreamSessionState(
        IAsyncStreamReader<ClientToServerMessage> requestStream, IServerStreamWriter<ServerToClientMessage> responseStream, ServerCallContext context)
    {
        // 1. Get essential information taskupdate send to the server
        // - cannot start until getting the vrlearningSessionId and deviceSerialNumber
        // - only handle event correspond with the vrLearningSessionId and deviceSerialNumber
        string vrLearningSessionId = null;
        string deviceSerialNumber = null;


        // Get the information on the first message, but not skip it since it contains important data.
        var vrLearningSessionIdGate = new TaskCompletionSource<bool>(false);
        Action<ClientToServerMessage> onFirstMessage = (message) =>
        {
            vrLearningSessionId = message.VrLearningSessionId;
            deviceSerialNumber = message.VrDeviceSerialNumber;
            _logger.LogInformation(
                "VR device stream connected. SessionId: {SessionId}, SerialNumber: {SerialNumber}",
                vrLearningSessionId,
                deviceSerialNumber);


            vrLearningSessionIdGate.TrySetResult(true);
        };

        try
        {
            // 2. Listening Background Task and Sending Background Task
            Task listeningTask = ListenForVRDeviceRedisMessagesAsync(requestStream, responseStream, onFirstMessage, context.CancellationToken);

            // 3. Main thread will stop right here until getting the vrLearningSessionId from the first message
            await vrLearningSessionIdGate.Task;

            Task sendingTask = SendServerRedisMessagesToVRDeviceAsync(responseStream, vrLearningSessionId!, context.CancellationToken);

            // 4. When all tasks complete, log disconnection
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

                // Handle set disconnected to device suddenly end the stream
                await _vrGlassVRLearningSessionService.SetDeviceStatusDisconnectedAsync(vrLearningSessionId, deviceSerialNumber);

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

                            // Publish TASKUPDATED event to the Redis Stream for processing updation later
                            await _vrGlassVRLearningSessionService.PublishTaskUpdateToStreamAsync(dto);

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
        string vrLearningSessionId,
        CancellationToken cancellationToken)

    {
        ISubscriber subscriber = _database.Multiplexer.GetSubscriber();
        var channel = Channel.CreateUnbounded<RedisValue>();

        await subscriber.SubscribeAsync(RedisChannel.Literal(AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_CHANNELS_NOTIFY_EVENTS_TO_VR), (redisChannel, message) =>
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

                // If not the event type, then continue, does not handle that event
                if (!redisEvent.isEventType)
                {
                    continue;
                }

                // Only handle messages for the current VR Learning Session ID
                if (!string.Equals(redisEvent.identifier, vrLearningSessionId, StringComparison.OrdinalIgnoreCase))
                {
                    // This message is for a different session, so we ignore it.
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
        finally
        {
            await subscriber.UnsubscribeAsync(RedisChannel.Literal(AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_CHANNELS_NOTIFY_EVENTS_TO_VR));
        }
    }
}
