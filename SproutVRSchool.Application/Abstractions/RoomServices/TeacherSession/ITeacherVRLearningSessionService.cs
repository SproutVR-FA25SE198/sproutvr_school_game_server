using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession.Dtos;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession;

public interface ITeacherVRLearningSessionService
{
    Task<CreateRoomResponseDto> CreateRoomAsync(CreateRoomRequestDto request);

    Task<ActivateRoomResponseDto> ActivateRoomAsync(ActivateRoomRequestDto request);

    Task<CancelRoomResponseDto> CancelRoomAsync(string vrLearningSessionId);

    /// <summary>
    /// Send INFO or WARNING notification to all devices in the room using PUB/SUB
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task SendNotificationAsync(SendNotificationRequestDto request);
}




