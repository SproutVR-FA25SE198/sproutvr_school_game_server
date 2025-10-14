using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SproutVRSchool.Application.Abstractions.RoomSession;

public interface IRoomSessionValidator
{
    Task<bool> IsRoomCodeExistsAsync(string roomCode);
    Task<bool> IsRoomSessionExistsAsync(string roomId);
    Task<bool> IsRoomSessionActiveAsync(string roomId);
}
