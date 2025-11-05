using MediatR;
using SproutVRSchool.Application.Abstractions.Data;
using SproutVRSchool.Application.Abstractions.FileServices;
using SproutVRSchool.Application.Abstractions.FileServices.Dtos;
using SproutVRSchool.Application.Exceptions.ContentSeedings;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Maps.Commands.ImportAccountsFromExcel;

public sealed class SAImportAccountsFromExcelCommandHandler(
    IFileValidationService fileValidationService,
    IExcelFileService excelFileService,
    IIdentityDbContextSeeder identityDbContextSeeder
    ) : IRequestHandler<SAImportAccountsFromExcelCommand, SAImportAccountsFromExcelCommandResponseDto>
{
    public async Task<SAImportAccountsFromExcelCommandResponseDto> Handle(SAImportAccountsFromExcelCommand request, CancellationToken cancellationToken)
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
        IReadOnlyList<TeacherAccountExcelRowDto> teacherAccounts = excelFileService.ReadTeachersExcel(request.ExcelFile);

        // 3. Seed teacher accounts to the database
        int totalNewSeeded = 0;
        foreach (TeacherAccountExcelRowDto teacherAccount in teacherAccounts)
        {
            if (await identityDbContextSeeder.SeedTeacherFromExcelFileAsync(teacherAccount))
            {
                totalNewSeeded++;
            }
        }

        // 4. Return response
        return new SAImportAccountsFromExcelCommandResponseDto(
            TotalNewSeeded: totalNewSeeded,
            Message: $"Successfully imported {totalNewSeeded} teacher accounts from the provided Excel file.");

    }
}


