using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession.Dtos;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession;

public interface ITeacherVRLearningSessionService
{
    Task<CreateRoomResponseDto> CreateRoomAsync(CreateRoomRequestDto request);

    Task<ActivateRoomResponseDto> ActivateRoomAsync(ActivateRoomRequestDto request);

    Task<CancelRoomResponseDto> CancelRoomAsync(string vrLearningSessionId);

    Task SendNotificationAsync(SendNotificationRequestDto request);
}

