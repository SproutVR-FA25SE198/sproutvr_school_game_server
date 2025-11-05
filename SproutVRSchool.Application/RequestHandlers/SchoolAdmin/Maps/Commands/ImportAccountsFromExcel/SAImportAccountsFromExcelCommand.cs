using MediatR;
using Microsoft.AspNetCore.Http;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Maps.Commands.ImportAccountsFromExcel;

public record SAImportAccountsFromExcelCommand(IFormFile ExcelFile) : IRequest<SAImportAccountsFromExcelCommandResponseDto>
{
}


