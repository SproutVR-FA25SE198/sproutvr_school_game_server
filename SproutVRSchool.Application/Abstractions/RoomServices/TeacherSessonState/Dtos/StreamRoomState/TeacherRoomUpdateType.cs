namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessonState.Dtos.StreamRoomState;

public enum TeacherRoomUpdateType
{
    Unknown = 0,
    RoomCreated = 1,
    RoomActivated = 2,
    RoomCancelled = 3,
    RoomEnded = 4,
    DeviceJoined = 5,
    DeviceDisconnected = 6,
    TaskUpdated = 7
}
