using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SproutVRSchool.Application.Abstractions.Services.VRGlassSession.Dtos;

public record TaskUpdateDto(
    string TaskId,
    string Question,
    bool IsCorrect,
    string AnswerText
    );
