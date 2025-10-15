using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Application.Abstractions.RoomSession;

namespace SproutVRSchool.Infrastructure.Redis.VRLearningSession;

public class VRLearningSessionValidator : IRoomSessionValidator
{
    public Task<bool> IsRoomCodeExistsAsync(string roomCode)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsRoomSessionActiveAsync(string roomId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsRoomSessionExistsAsync(string roomId)
    {
        throw new NotImplementedException();
    }
}
