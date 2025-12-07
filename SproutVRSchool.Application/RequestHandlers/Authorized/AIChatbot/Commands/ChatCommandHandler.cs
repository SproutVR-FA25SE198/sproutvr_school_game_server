using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SproutVRSchool.Application.Abstractions.AIServices;
using SproutVRSchool.Application.RequestHandlers.Auth.Queries.GetCurrentUser;
using SproutVRSchool.Domain.Entities.Identities;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.AIChatbot.Commands;

public class ChatCommandHandler : IRequestHandler<ChatCommand, ChatResponseDto>
{
    private readonly IChatbotService _chatbotService;
    private readonly IMediator _mediator;
    private readonly UserManager<UserAccount> _userManager;
    public ChatCommandHandler(
        IChatbotService chatbotService,
        IMediator mediator,         
        UserManager<UserAccount> userManager)
    {
        _chatbotService = chatbotService;
        _mediator = mediator;
        _userManager = userManager;
    }
    public async Task<ChatResponseDto> Handle(ChatCommand request, CancellationToken cancellationToken)
    {
        AuthGetCurrentUserCommandResponseDto currentUser = await _mediator.Send(new AuthGetCurrentUserCommand(), cancellationToken);
        string? organizationId = await GetOrganizationIdAsync();
        ChatResponseDto response = await _chatbotService.Chat(request.Message, currentUser.UserId, organizationId);
        return response;
    }

    private async Task<string> GetOrganizationIdAsync()
    {

        Domain.Entities.Identities.SchoolAdmin? admin = await _userManager.Users.OfType<Domain.Entities.Identities.SchoolAdmin>().FirstOrDefaultAsync();

        if (admin == null || admin.OrganizationId == Guid.Empty)
        {
            throw new Exception("Missing organization information!");
        }

        return admin.OrganizationId.ToString();
    }
}
