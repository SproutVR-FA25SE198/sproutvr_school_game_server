using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Accounts.Commands.ImportAccountsFromExcel;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.SchoolAdmin.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/school-admin/accounts")]
public sealed class AccountsController(IMediator mediator) : BaseApiController
{
    // ========================
    // === GETs
    // ========================

    // ========================
    // === POSTs
    // ========================

    // POST: api/v1/school-admin/accounts/import
    [HttpPost("import")]
    public async Task<IActionResult> SeedFromExcel([FromForm] SAImportAccountsFromExcelCommand request)
    {
        SAImportAccountsFromExcelCommandResponseDto response = await mediator.Send(request);
        return Ok(response);
    }

    // ========================
    // === PUTs
    // ========================

    // ========================
    // === PATCHs
    // ========================
}


