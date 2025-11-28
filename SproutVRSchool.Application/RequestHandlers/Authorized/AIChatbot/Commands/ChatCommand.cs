using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.AIChatbot.Commands;
public class ChatCommand : IRequest<ChatResponseDto>
{
    public string Message { get; set; }
    public ChatCommand(string message)
    {
        Message = message;
    }
}
