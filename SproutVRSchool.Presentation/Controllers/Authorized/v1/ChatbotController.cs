using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.RequestHandlers.Authorized.AIChatbot.Commands;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Authorized.v1;

[Authorize]
[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/authorized/chatbot")]
public sealed class ChatbotController(IMediator mediator) : BaseApiController
{
    // =======================
    // === GETs
    // =======================

    // ========================
    // === POSTs
    // ========================

    // POST: api/v1/authorized/chatbot
    [HttpPost]
    [RequestTimeout(120000)]
    public async Task<IActionResult> Post([FromBody] ChatCommand chatCommand)
    {
        return Ok(await mediator.Send(chatCommand));
    }

    // ========================
    // === PUTs
    // ========================

    // ========================
    // === PATCHs
    // ========================

    // ========================
    // === DELETEs
    // ========================
}
