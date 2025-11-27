using Microsoft.AspNetCore.Http;
using SproutVRSchool.Application.Abstractions.FileServices.Dtos;

namespace SproutVRSchool.Application.Abstractions.FileServices;

public interface IExcelFileService
{
    IReadOnlyList<TeacherAccountExcelRowDto> ReadTeachersExcel(IFormFile excelFile);

    IReadOnlyList<VRDeviceExcelRowDto> ReadVRDevicesExcel(IFormFile excelFile);
}
