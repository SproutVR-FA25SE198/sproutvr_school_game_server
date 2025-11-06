using MediatR;
using Microsoft.AspNetCore.Http;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Accounts.Commands.ImportAccountsFromExcel;

public record SAImportAccountsFromExcelCommand(IFormFile ExcelFile) : IRequest<SAImportAccountsFromExcelCommandResponseDto>
{
}


