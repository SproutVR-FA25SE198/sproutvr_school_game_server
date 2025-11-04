using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Auth.Commands.Login;

public sealed record AuthLoginCommand(string Identifier, string Password) 
    : IRequest<AuthLoginCommandResponseDto>
{
}
