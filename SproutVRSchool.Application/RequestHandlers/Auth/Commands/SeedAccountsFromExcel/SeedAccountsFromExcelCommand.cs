using MediatR;
using Microsoft.AspNetCore.Http;

namespace SproutVRSchool.Application.RequestHandlers.Auth.Commands.SeedAccountsFromExcel;

public record SeedAccountsFromExcelCommand(IFormFile ExcelFile) : IRequest<SeedAccountsFromExcelResponseDto>
{
}


