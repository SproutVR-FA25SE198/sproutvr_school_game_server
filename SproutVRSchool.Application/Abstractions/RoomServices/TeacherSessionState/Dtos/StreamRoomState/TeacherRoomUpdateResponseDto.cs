using System.Globalization;
using LearningSession.V1;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;

public class TeacherRoomUpdateResponseDto
{
    // =============================
    // === Fields
    // =============================

    public string VrLearningSessionId { get; set; }

    public TeacherRoomUpdateType UpdateType { get; init; }

    public RoomCancelledDto? RoomCancelled { get; init; }
    public RoomEndedDto? RoomEnded { get; init; }
    public DeviceJoinedDto? DeviceJoined { get; init; }
    public DeviceDisconnectedDto? DeviceDisconnected { get; init; }
    public TaskUpdatedDto? TaskUpdated { get; init; }

    // =============================
    // === Methods
    // =============================

    public static TeacherRoomUpdateResponse MapToGrpcResponse(TeacherRoomUpdateResponseDto dto)
    {
        var response = new TeacherRoomUpdateResponse
        {
            VrLearningSessionId = dto.VrLearningSessionId
        };

        // Map the specific update type
        if (dto.RoomCancelled is not null)
        {
            response.RoomCancelled = new RoomCancelled
            {
                Reason = dto.RoomCancelled.Reason,
                RoomStatus = dto.RoomCancelled.RoomStatus
            };
        }
        else if (dto.RoomEnded is not null)
        {
            response.RoomEnded = new RoomEnded
            {
                Reason = dto.RoomEnded.Reason,
                RoomStatus = dto.RoomEnded.RoomStatus
            };
        }
        else if (dto.DeviceJoined is not null)
        {
            response.DeviceJoined = new DeviceJoined
            {
                VrDeviceSerialNumber = dto.DeviceJoined.VrDeviceSerialNumber,
                ConnectionStatus = dto.DeviceJoined.ConnectionStatus
            };
        }
        else if (dto.DeviceDisconnected is not null)
        {
            response.DeviceDisconnected = new DeviceDisconnected
            {
                VrDeviceSerialNumber = dto.DeviceDisconnected.VrDeviceSerialNumber,
                ConnectionStatus = dto.DeviceDisconnected.ConnectionStatus
            };
        }
        else if (dto.TaskUpdated is not null)
        {
            response.TaskUpdated = new TaskUpdated
            {
                VrDeviceSerialNumber = dto.TaskUpdated.VrDeviceSerialNumber,
                VrTaskId = dto.TaskUpdated.VrTaskId,
                IsCompleted = dto.TaskUpdated.IsCompleted,
                IsCorrect = dto.TaskUpdated.IsCorrect,
                TaskUpdatedStatus = dto.TaskUpdated.Status,
                CompletionTimeAtVietnam = dto.TaskUpdated.CompletionTimeAtVietNam.ToString("o", CultureInfo.InvariantCulture)
            };
        }

        return response;
    }
}

