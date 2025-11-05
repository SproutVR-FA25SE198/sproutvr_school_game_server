using MediatR;
using SproutVRSchool.Application.Abstractions.Data;
using SproutVRSchool.Application.Abstractions.FileServices;
using SproutVRSchool.Application.Abstractions.FileServices.Dtos;
using SproutVRSchool.Application.Exceptions.ContentSeedings;

namespace SproutVRSchool.Application.RequestHandlers.Auth.Commands.SeedAccountsFromExcel;

public sealed class SeedAccountsFromExcelCommandHandler(
    IFileValidationService fileValidationService,
    IExcelFileService excelFileService,
    IIdentityDbContextSeeder identityDbContextSeeder
    ) : IRequestHandler<SeedAccountsFromExcelCommand, SeedAccountsFromExcelResponseDto>
{
    public async Task<SeedAccountsFromExcelResponseDto> Handle(SeedAccountsFromExcelCommand request, CancellationToken cancellationToken)
    {
        // 1. File Validation
        if (!fileValidationService.IsFileValid(request.ExcelFile))
        {
            throw new SvrFileNotSupportedException("The provided Excel file is invalid or empty.");
        }

        if (!fileValidationService.IsExcelFile(request.ExcelFile))
        {
            throw new SvrFileNotSupportedException("The provided file is not a supported Excel file.");
        }

        // 2. Process Excel File, get the readonly list
        IReadOnlyList<TeacherAccountExcelRowDto> teacherAccounts = excelFileService.ReadTeacherExcel(request.ExcelFile);

        // 3. Seed teacher accounts to the database
        int newAccountsAdded = 0;
        foreach (TeacherAccountExcelRowDto teacherAccount in teacherAccounts)
        {
            if (await identityDbContextSeeder.SeedTeacherFromExcelFileAsync(teacherAccount))
            {
                newAccountsAdded++;
            }
        }

        // 4. Return response
        return new SeedAccountsFromExcelResponseDto(
            Message: $"Successfully seeded {newAccountsAdded} teacher accounts from the provided Excel file.");

    }
}


