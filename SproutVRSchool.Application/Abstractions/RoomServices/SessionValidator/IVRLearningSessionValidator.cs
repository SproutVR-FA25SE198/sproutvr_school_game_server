namespace SproutVRSchool.Application.Abstractions.RoomServices.SessionValidator;

public interface IVRLearningSessionValidator
{
    Task<ValidationResult> ValidateJoinAttemptAsync(
            string roomCode,
            string deviceSerialNumber);
}
