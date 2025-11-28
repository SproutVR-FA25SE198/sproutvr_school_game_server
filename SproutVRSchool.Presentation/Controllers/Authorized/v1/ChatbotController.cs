using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.RequestHandlers.Authorized.AIChatbot.Commands;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Authorized.v1;
[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/authorized/chatbot")] 
public class ChatbotController(IMediator mediator) : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ChatCommand chatCommand)
    {
        return Ok(await mediator.Send(chatCommand));
    }
}
