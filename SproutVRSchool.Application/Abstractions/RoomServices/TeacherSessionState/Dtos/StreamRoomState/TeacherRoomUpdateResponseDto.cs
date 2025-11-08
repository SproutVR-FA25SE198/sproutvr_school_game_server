namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;

public class TeacherRoomUpdateResponseDto
{
    public string VrLearningSessionId { get; set; } = default!;

    public TeacherRoomUpdateType Type { get; set; }

    public object? Payload { get; set; } // json payload, depends on the type
}
