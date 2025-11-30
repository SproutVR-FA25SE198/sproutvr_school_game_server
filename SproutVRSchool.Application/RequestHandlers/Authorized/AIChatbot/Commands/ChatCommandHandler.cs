using MediatR;
using SproutVRSchool.Application.Abstractions.AIServices;
using SproutVRSchool.Application.RequestHandlers.Auth.Queries.GetCurrentUser;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.AIChatbot.Commands;

public class ChatCommandHandler : IRequestHandler<ChatCommand, ChatResponseDto>
{
    private readonly IChatbotService _chatbotService;
    private readonly IMediator _mediator;
    public ChatCommandHandler(
        IChatbotService chatbotService,
        IMediator mediator)
    {
        _chatbotService = chatbotService;
        _mediator = mediator;
    }
    public async Task<ChatResponseDto> Handle(ChatCommand request, CancellationToken cancellationToken)
    {
        AuthGetCurrentUserCommandResponseDto currentUser = await _mediator.Send(new AuthGetCurrentUserCommand(), cancellationToken);
        ChatResponseDto response = await _chatbotService.Chat(request.Message, currentUser.UserId, currentUser.OrganizationId);
        return response;
    }
}
