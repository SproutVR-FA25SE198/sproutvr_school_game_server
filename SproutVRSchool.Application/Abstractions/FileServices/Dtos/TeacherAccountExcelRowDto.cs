namespace SproutVRSchool.Application.Abstractions.FileServices.Dtos;

public sealed record TeacherAccountExcelRowDto(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string DateOfBirth,
    string DefaultPassword);
