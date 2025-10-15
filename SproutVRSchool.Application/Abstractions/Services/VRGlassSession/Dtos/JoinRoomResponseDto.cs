using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LearningSession.V1.JoinRoomResponse.Types;

namespace SproutVRSchool.Application.Abstractions.Services.VRGlassSession.Dtos;

public record JoinRoomResponseDto(
    JoinStatus Status,
    string Message,
    string? LearningSessionId = null,
    string? PresetJsonContent = null);
