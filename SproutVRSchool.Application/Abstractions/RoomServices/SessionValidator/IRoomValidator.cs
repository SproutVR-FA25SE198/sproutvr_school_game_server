namespace SproutVRSchool.Application.Abstractions.RoomServices.SessionValidator;

public interface IRoomValidator
{
    Task<ValidationResultDto> ValidateJoinAttemptAsync(
            string roomCode,
            string deviceSerialNumber);

    // UNDONE: will add more validation here for other methods
}
