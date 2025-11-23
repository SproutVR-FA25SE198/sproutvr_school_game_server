namespace SproutVRSchool.Application.Abstractions.RoomServices.CodeGenerator;

public interface IRoomCodeGeneratorService
{
    string GenerateCode(int length = 6);
}
