using Grpc.Core;
using LearningSession.V1;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessonState;

namespace SproutVRSchool.Presentation.Grpc;

public class GrpcTeacherVRLearningSessionStateService(
    ITeacherVRLearningSessionStateService teacherVRLearningSessionStateService
    )
    : TeacherSessionRealTimeStateManagement.TeacherSessionRealTimeStateManagementBase
{
    public override async Task<GetRoomStateResponse> GetRoomState(GetRoomStateRequest request, ServerCallContext context)
    {
        return await teacherVRLearningSessionStateService.GetRoomStateAsync(request.VrLearningSessionId);
    }
}
