namespace SproutVRSchool.Application.Abstractions.RoomServices.CodeGenerator;

public interface ICodeGeneratorService
{
    string GenerateCode(int length = 6);
}
