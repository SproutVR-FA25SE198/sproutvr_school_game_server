using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using SproutVRSchool.Application.Abstractions.FileServices;
using SproutVRSchool.Application.Abstractions.FileServices.Dtos;
using SproutVRSchool.Application.Exceptions.ContentSeedings;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Infrastructure.Services.FileServices;

public class ExcelFileService(
    ILogger<ExcelFileService> excelFileService) : IExcelFileService
{
    public IReadOnlyList<TeacherAccountExcelRowDto> ReadTeachersExcel(IFormFile excelFile)
    {
        // 1. Load the workbook to the memory for reading
        using Stream stream = excelFile.OpenReadStream();
        using var package = new ExcelPackage(stream);

        // 2. Get the sheet named "Accounts", or get the first sheet if it does not exist
        ExcelWorksheet worksheet = package.Workbook.Worksheets[AppCts.Files.EXCEL_WORKSHEET_ACCOUNTS]
            ?? package.Workbook.Worksheets.FirstOrDefault()
            ?? throw new SvrFileNotSupportedException("Excel file does not contain a valid 'Accounts' worksheet.");

        // 3. Validate Data Rows
        int totalRows = worksheet.Dimension?.Rows ?? 0;
        if (totalRows < 2)
        {
            throw new SvrFileNotSupportedException("Excel file does not contain any data rows.");
        }

        // 4. Get the data rows
        var result = new HashSet<TeacherAccountExcelRowDto>();

        for (int row = 2; row <= totalRows; row++)
        {
            string firstName = worksheet.Cells[row, 1].Text?.Trim() ?? string.Empty;
            string lastName = worksheet.Cells[row, 2].Text?.Trim() ?? string.Empty;
            string username = worksheet.Cells[row, 3].Text?.Trim() ?? string.Empty;
            string email = worksheet.Cells[row, 4].Text?.Trim() ?? string.Empty;
            string dateOfBirth = worksheet.Cells[row, 5].Text?.Trim() ?? string.Empty;
            string defaultPassword = worksheet.Cells[row, 6].Text?.Trim() ?? string.Empty;

            // Must have Email
            if (string.IsNullOrWhiteSpace(email))
            {
                excelFileService.LogWarning("Skipping row {Row} due to missing email.", row);
                continue;
            }

            // Must have UserName
            if (string.IsNullOrWhiteSpace(username))
            {
                excelFileService.LogWarning("Skipping row {Row} due to missing username.", row);
                continue;
            }

            // Must have default password
            if (string.IsNullOrWhiteSpace(defaultPassword))
            {
                excelFileService.LogWarning("Skipping row {Row} due to missing password.", row);
                continue;
            }

            var teacherAccount = new TeacherAccountExcelRowDto(
                FirstName: firstName,
                LastName: lastName,
                UserName: username,
                Email: email,
                DateOfBirth: dateOfBirth,
                DefaultPassword: defaultPassword);

            result.Add(teacherAccount);
        }

        return result.ToList();
    }

    public IReadOnlyList<VRDeviceExcelRowDto> ReadVRDevicesExcel(IFormFile excelFile)
    {
        // 1. Load the workbook to the memory for reading
        using Stream stream = excelFile.OpenReadStream();
        using var package = new ExcelPackage(stream);

        // 2. Get the sheet named "VRDevices", or get the first sheet if it does not exist
        ExcelWorksheet worksheet = package.Workbook.Worksheets[AppCts.Files.EXCEL_WORKSHEET_VRDEVICES]
            ?? package.Workbook.Worksheets.FirstOrDefault()
            ?? throw new SvrFileNotSupportedException("Excel file does not contain a valid 'VRDevices' worksheet.");

        // 3. Validate Data Rows
        int totalRows = worksheet.Dimension?.Rows ?? 0;
        if (totalRows < 2)
        {
            throw new SvrFileNotSupportedException("Excel file does not contain any data rows.");
        }

        // 4. Get the data rows
        var result = new HashSet<VRDeviceExcelRowDto>();

        for (int row = 2; row <= totalRows; row++)
        {
            string name = worksheet.Cells[row, 1].Text?.Trim() ?? string.Empty;
            string serialNumber = worksheet.Cells[row, 2].Text?.Trim() ?? string.Empty;

            // Must have device name
            if (string.IsNullOrWhiteSpace(name))
            {
                excelFileService.LogWarning("Skipping row {Row} due to missing device name.", row);
                continue;
            }

            // Must have device's serial number
            if (string.IsNullOrWhiteSpace(serialNumber))
            {
                excelFileService.LogWarning("Skipping row {Row} due to missing device serial number.", row);
                continue;
            }

            var vrDevice = new VRDeviceExcelRowDto(
                DeviceName: name,
                SerialNumber: serialNumber);

            result.Add(vrDevice);
        }

        return result.ToList();
    }
}
