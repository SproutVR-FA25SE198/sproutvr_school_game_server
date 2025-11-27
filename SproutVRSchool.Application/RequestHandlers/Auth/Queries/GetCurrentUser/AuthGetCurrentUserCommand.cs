using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Auth.Queries.GetCurrentUser;

public sealed record AuthGetCurrentUserCommand : IRequest<AuthGetCurrentUserCommandResponseDto>;

