using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;

public record PublishTaskUpdateRequestDto
    (string VrLearningSessionId,
    string VrDeviceSerialNumber,
    string VrTaskId,
    bool IsCompleted,
    bool IsCorrect)
{
}
