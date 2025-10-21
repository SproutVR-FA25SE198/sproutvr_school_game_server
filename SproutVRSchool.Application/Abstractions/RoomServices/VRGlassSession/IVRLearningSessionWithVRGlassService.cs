using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;

namespace SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession;

public interface IVRLearningSessionWithVRGlassService
{
    Task<JoinRoomResponseDto> JoinRoomAsync(JoinRoomRequestDto joinRoomRequestDto);

    Task PublishTaskUpdateToStreamAsync(PublishTaskUpdateRequestDto publishTaskUpdateRequestDto);
}


