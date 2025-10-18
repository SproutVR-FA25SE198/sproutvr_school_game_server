using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;

namespace SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession;

public interface IVRLearningSessionWithVRGlassService
{
    Task<JoinRoomResponseDto> JoinRoomAsync(JoinRoomRequestDto joinRoomRequestDto);

    Task PublishTaskUpdateToStreamAsync(PublishTaskUpdateRequestDto publishTaskUpdateRequestDto);

    // THIS IS FOR A USEAGE OF PUB/SUB TO HANDLE TASK UPDATES FROM VR GLASS DEVICES
#pragma warning disable S125 // Sections of code should not be commented out
    //Task<TaskUpdateResponseDto> HandleTaskUpdateAsync(TaskUpdateRequestDto request);
#pragma warning restore S125 // Sections of code should not be commented out
}


