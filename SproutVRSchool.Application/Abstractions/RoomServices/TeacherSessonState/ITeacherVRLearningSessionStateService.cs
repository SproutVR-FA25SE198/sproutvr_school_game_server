using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LearningSession.V1;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessonState.Dtos.StreamRoomState;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessonState;

public interface ITeacherVRLearningSessionStateService
{
    /// <summary>
    /// Get the iniitial room state for the given vr learning session
    /// </summary>
    /// <param name="vrLearningSessionId"></param>
    /// <returns></returns>
    Task<GetRoomStateResponse> GetRoomStateAsync(string vrLearningSessionId);

    /// <summary>
    /// Subscribe to redis pub/sub  events for the given vr learning session
    /// and update partially the room state
    /// </summary>
    /// <param name="vrLearningSessionId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<TeacherRoomUpdateResponseDto> StreamRoomUpdatesAsync(
        string vrLearningSessionId,
        CancellationToken cancellationToken);
}
