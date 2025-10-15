using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Application.Abstractions.Services.VRGlassSession.Dtos;
using SproutVRSchool.Domain.Models.VRLearningSession;

namespace SproutVRSchool.Application.Abstractions.Services.SessionValidator;

/// <summary>
/// This interface will validate the learning session state
/// </summary>
public class ValidationResult
{
    public bool IsValid { get; init; }
    public JoinRoomResponseDto JoinRoomResponseDto { get; init; }

    // after validation, return the learning session info to avoid fetching twice
    public ModelVRLearningSession? VrLearningSession { get; init; }
}
