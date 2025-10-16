using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SproutVRSchool.Application.Abstractions.Services.VRGlassSession.Dtos;

public record TaskUpdateDto(
    string TaskId,
    bool IsCompleted = false,
    bool? IsCorrect = null);
