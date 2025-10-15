using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SproutVRSchool.Application.Abstractions.RoomSession;

public interface IRoomSessionValidator
{
    Task<ValidationResult> ValidateRoomCodeAsync(string roomCode);
    Task<ValidationResult> ValidateLearningSessionRoomAsync(string roomId, string deviceIdentifier);
}
