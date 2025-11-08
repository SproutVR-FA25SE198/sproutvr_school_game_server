using Grpc.Core;
using LearningSession.V1;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.GetRoomState;

namespace SproutVRSchool.Presentation.Grpc;

public class GrpcTeacherVRLearningSessionStateService(
    ITeacherVRLearningSessionStateService teacherVRLearningSessionStateService
    )
    : TeacherSessionRealTimeStateManagement.TeacherSessionRealTimeStateManagementBase
{
    public override async Task<GetRoomStateResponse> GetRoomState(GetRoomStateRequest request, ServerCallContext context)
    {
        var requestDto = GetRoomStateRequestDto.MapFromGrpcRequest(request);
        GetRoomStateResponseDto roomState = await teacherVRLearningSessionStateService.GetRoomStateAsync(requestDto);
        return GetRoomStateResponseDto.MapToGrpcResponse(roomState);
    }
}
