using Grpc.Core;
using LearningSession.V1;
using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession;
using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;

namespace SproutVRSchool.Presentation.Grpc;

public sealed class GrpcVRGlassVRLearningSessionService : VRGlassSessionManagement.VRGlassSessionManagementBase
{
    // ===============================
    // === Fields
    // ===============================

    private readonly IVRLearningSessionWithVRGlassService _vrLearningSessionWithVRGlassService;
    private readonly ILogger<GrpcVRGlassVRLearningSessionService> _logger;

    // ==============================
    // === Constructors
    // ==============================

    public GrpcVRGlassVRLearningSessionService(
        ILogger<GrpcVRGlassVRLearningSessionService> logger,
        IVRLearningSessionWithVRGlassService vrLearningSessionWithVRGlassService)
    {
        _vrLearningSessionWithVRGlassService = vrLearningSessionWithVRGlassService;
        _logger = logger;
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
        _logger.LogInformation("VR device stream connected.");

        if (!await requestStream.MoveNext(context.CancellationToken))
        {
            return;
        }

        ClientToServerMessage initialMessage = requestStream.Current;
        string sessionId = initialMessage.VrLearningSessionId;

        //  Listening Background Task and Sending Background Task
        Task listeningTask = ListenForClientMessagesAsync(requestStream, context.CancellationToken);
        Task sendingTask = SendServerMessagesAsync(responseStream, context.CancellationToken);

        await Task.WhenAll(listeningTask, sendingTask);
        _logger.LogInformation("VR device stream disconnected for Session ID: {SessionId}", sessionId);
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
        CancellationToken cancellationToken)
    {
        await foreach (ClientToServerMessage? message in requestStream.ReadAllAsync(cancellationToken))
        {
            if (message.PayloadCase == ClientToServerMessage.PayloadOneofCase.TaskUpdate)
            {
                TaskUpdate taskUpdate = message.TaskUpdate;
                var dto = new PublishTaskUpdateRequestDto(
                    message.VrLearningSessionId,
                    message.VrDeviceSerialNumber,
                    taskUpdate.VrTaskId,
                    taskUpdate.IsCompleted,
                    taskUpdate.IsCorrect);
                await _vrLearningSessionWithVRGlassService.PublishTaskUpdateToStreamAsync(dto);
            }
        }
    }

    /// <summary>
    /// This task is dedicated to SENDING messages TO the client.
    /// </summary>
    /// <param name="responseStream"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task SendServerMessagesAsync(
        IServerStreamWriter<ServerToClientMessage> responseStream,
        CancellationToken cancellationToken)
    {
        // This is where you would subscribe to Redis Pub/Sub or poll a Redis Stream.
        while (!cancellationToken.IsCancellationRequested)
        {
            var notification = new ServerToClientMessage
            {
                Notification = new NotificationSignal
                {
                    Text = $"Làm bài đi thằng nhóc! at {DateTime.UtcNow:T}",
                    Severity = NotificationSignal.Types.Severity.Info
                }
            };

            await responseStream.WriteAsync(notification, cancellationToken);
        }
    }
}
