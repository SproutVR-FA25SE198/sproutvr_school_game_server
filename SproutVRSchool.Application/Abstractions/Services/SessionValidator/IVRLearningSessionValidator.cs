using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SproutVRSchool.Application.Abstractions.Services.SessionValidator;

public interface IVRLearningSessionValidator
{
    Task<ValidationResult> ValidateJoinAttemptAsync(
            string roomCode,
            string deviceSerialNumber);
}
