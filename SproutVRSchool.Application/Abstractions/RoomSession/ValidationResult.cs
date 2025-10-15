using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SproutVRSchool.Application.Abstractions.RoomSession;

public record ValidationResult(
    bool IsValid = false,
    string? RoomId = null,
    JoinRoomResult? ErrorResponse = null)
{
}
