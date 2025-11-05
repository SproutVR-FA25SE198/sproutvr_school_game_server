namespace SproutVRSchool.Application.Abstractions.RoomServices.SessionValidator;

public interface IVRLearningSessionValidator
{
    Task<ValidationResultDto> ValidateJoinAttemptAsync(
            string roomCode,
            string deviceSerialNumber);
}
