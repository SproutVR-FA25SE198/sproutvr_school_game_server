using LearningSession.V1;

namespace SproutVRSchool.Application.Abstractions.Services.TeacherSession.Dtos;

public record CreateRoomRequestDto(
    string TeacherId,
    string VrLessionId,
    string SessionName)
{
    public static CreateRoomRequestDto MapFromGrpcRequest(CreateRoomRequest request)
    {
        return new CreateRoomRequestDto(
            request.TeacherId,
            request.VrLessonId,
            request.SessionName);
    }
}
