using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SproutVRSchool.Application.Abstractions.RoomServices.Cleanup;

internal interface IRoomCleanUpService
{
    /// <summary>
    /// Clean up all remaining room's data after saving the room successfully
    /// </summary>
    /// <returns></returns>
    Task<bool> CleanUpRoomStateAsync(string roomCode, string vrLearningSessionId);
}
