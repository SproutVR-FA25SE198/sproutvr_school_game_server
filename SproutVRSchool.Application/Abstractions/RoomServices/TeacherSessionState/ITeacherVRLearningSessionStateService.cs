using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LearningSession.V1;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.GetRoomState;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState;

public interface ITeacherVRLearningSessionStateService
{
    /// <summary>
    /// Get the iniitial room state for the given vr learning session
    /// </summary>
    /// <param name="vrLearningSessionId"></param>
    /// <returns></returns>
    Task<GetRoomStateResponseDto> GetRoomStateAsync(GetRoomStateRequestDto getRoomStateRequestDto);

    /// <summary>
    /// Subscribe to redis pub/sub  events for the given vr learning session
    /// and update partially the room state
    /// </summary>
    /// <param name="vrLearningSessionId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<TeacherRoomUpdateResponseDto> StreamRoomUpdatesAsync(
        TeacherRoomUpdateRequestDto teacherRoomUpdateRequestDto,
        CancellationToken cancellationToken);

    /// <summary>
    /// Publish Room Cancelled event to the given vr learning session
    /// </summary>
    /// <param name="vrLearningSessionId"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task PublishRoomCancelledAsync(string vrLearningSessionId, RoomCancelledDto dto);

    /// <summary>
    /// Publishes a notification indicating that a VR learning session room has ended.
    /// </summary>
    /// <param name="vrLearningSessionId">The unique identifier of the VR learning session whose room has ended. Cannot be null or empty.</param>
    /// <param name="dto">An object containing details about the ended room. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous publish operation.</returns>
    Task PublishRoomEndedAsync(string vrLearningSessionId, RoomEndedDto dto);

    /// <summary>
    /// Publish Device Joined event to the given vr learning session
    /// </summary>
    /// <param name="vrLearningSessionId"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task PublishDeviceJoinedAsync(string vrLearningSessionId, DeviceJoinedDto dto);

    /// <summary>
    /// Publishes a notification indicating that a device has been disconnected from a VR learning session.
    /// </summary>
    /// <param name="vrLearningSessionId">The unique identifier of the VR learning session from which the device was disconnected. Cannot be null or
    /// empty.</param>
    /// <param name="dto">An object containing details about the disconnected device. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous publish operation.</returns>
    Task PublishDeviceDisconnectedAsync(string vrLearningSessionId, DeviceDisconnectedDto dto);

    /// <summary>
    /// Publishes a notification that a task has been updated within the specified VR learning session.
    /// </summary>
    /// <param name="vrLearningSessionId">The unique identifier of the VR learning session in which the task update occurred. Cannot be null or empty.</param>
    /// <param name="dto">An object containing the details of the updated task. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous publish operation.</returns>
    Task PublishTaskUpdatedAsync(string vrLearningSessionId, TaskUpdatedDto dto);
}

