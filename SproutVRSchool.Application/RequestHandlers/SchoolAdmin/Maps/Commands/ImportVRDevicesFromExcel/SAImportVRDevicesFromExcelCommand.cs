using MediatR;
using Microsoft.AspNetCore.Http;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Maps.Commands.ImportVRDevicesFromExcel;

public sealed record SAImportVRDevicesFromExcelCommand(IFormFile ExcelFile)
    : IRequest<SAImportVRDevicesFromExcelCommandResponseDto>
{
}


