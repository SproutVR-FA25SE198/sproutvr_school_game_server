using Grpc.Core;
using LearningSession.V1;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.GetRoomState;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;
using SproutVRSchool.Domain.Entities.VRLearningSessions;

namespace SproutVRSchool.Presentation.Grpc;

public class GrpcTeacherVRLearningSessionStateService(
    ITeacherVRLearningSessionStateService teacherVRLearningSessionStateService,
    ILogger<GrpcTeacherVRLearningSessionStateService> logger
    )
    : TeacherSessionRealTimeStateManagement.TeacherSessionRealTimeStateManagementBase
{
    public override async Task<GetRoomStateResponse> GetRoomState(GetRoomStateRequest request, ServerCallContext context)
    {
        var requestDto = GetRoomStateRequestDto.MapFromGrpcRequest(request);
        GetRoomStateResponseDto roomState = await teacherVRLearningSessionStateService.GetRoomStateAsync(requestDto);
        return GetRoomStateResponseDto.MapToGrpcResponse(roomState);
    }

    public override async Task StreamTeacherRoomState(TeacherRoomStreamRequest request, IServerStreamWriter<TeacherRoomUpdateResponse> responseStream, ServerCallContext context)
    {
        var requestDto = TeacherRoomUpdateRequestDto.MapFromGrpcRequest(request);

        logger.LogInformation("Starting teacher room stream for session {SessionId}", requestDto.VrLearningSessionId);

        // 1. Subscribe to room updates, listening and reading from the incoming event type
        //    - Coroutine technique, won't stop until the connection has ended
        //    - Stop conditions:
        //      + stream closes
        //      + server cancels via context.CancellationToken
        //      + room ends
        await foreach (TeacherRoomUpdateResponseDto update in teacherVRLearningSessionStateService.StreamRoomUpdatesAsync(requestDto, context.CancellationToken))
        {
            TeacherRoomUpdateResponse grpcResponse = TeacherRoomUpdateResponseDto.MapToGrpcResponse(update);
            await responseStream.WriteAsync(grpcResponse);

            // 2. Log the event pushed to the teacher client
            logger.LogInformation("Pushed {EventType} event to teacher client for session {SessionId}",
                grpcResponse.PayloadCase, requestDto.VrLearningSessionId);
        }

        logger.LogInformation("Teacher room stream ended for session {SessionId}", requestDto.VrLearningSessionId);
    }
}
