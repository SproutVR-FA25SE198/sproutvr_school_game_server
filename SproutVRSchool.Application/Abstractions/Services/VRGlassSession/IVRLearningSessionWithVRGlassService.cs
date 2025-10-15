using SproutVRSchool.Application.Abstractions.Services.VRGlassSession.Dtos;

namespace SproutVRSchool.Application.Abstractions.Services.VRGlassSession;

public interface IVRLearningSessionWithVRGlassService
{
    Task<JoinRoomResponseDto> JoinRoomAsync(JoinRoomRequestDto joinRoomRequestDto);
    Task PublishVrDeviceEventAsync(PublishEventRequestDto deviceEvent);
}


