namespace SproutVRSchool.Application.Abstractions.RoomSession;

public interface IRoomSession
{
    Task<CreateRoomResult> CreateRoomAsync(Guid ownerId, Guid? vrLessionId, string sessionName, int maxVrDevices);
    Task<CreateRoomResult> ScheduleRoomAsync(string roomId, DateTime startTimeUtc, int durationInMinutes);
    Task<JoinRoomResult> JoinRoomAsync(string roomCode, string deviceIdentifier, string deviceName);
    Task<string> EndRoomAsync(string roomId);
}



