namespace SproutVRSchool.Application.RequestHandlers.Auth.Commands.Login;

public sealed record AuthLoginCommandResponseDto(
    string AccessToken,
    DateTimeOffset ExpiredAtVietNam
);
