using SproutVRSchool.Application.RequestHandlers.Authorized.AIChatbot.Commands;

namespace SproutVRSchool.Application.Abstractions.AIServices;
public interface IChatbotService
{
    Task<ChatResponseDto> Chat(string chatMessage, Guid userId, string? organizationId);
}
