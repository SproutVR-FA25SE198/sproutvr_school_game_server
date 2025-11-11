using System.Collections.ObjectModel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Domain.Entities.Identities;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Accounts.Queries.GetTeacherAccountById;

public sealed class AuthorizedGetTeacherAccountByIdQueryHandler(
    UserManager<UserAccount> userManager
    )
    : IRequestHandler<AuthorizedGetTeacherAccountByIdQuery, AuthorizedGetTeacherAccountByIdQueryResponseDto>
{
    public async Task<AuthorizedGetTeacherAccountByIdQueryResponseDto> Handle(
        AuthorizedGetTeacherAccountByIdQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Get teacher by ID, and include Lessons, VRLearningSessions
        Domain.Entities.Identities.Teacher? teacher = await userManager.Users
            .OfType<Domain.Entities.Identities.Teacher>()
            .AsNoTracking()
            .Include(t => t.Lessons)
            .Include(t => t.VRLearningSessions)
            .FirstOrDefaultAsync(t => t.Id == request.TeacherId, cancellationToken)
            ?? throw new SvrResourceNotFoundException(
                $"Teacher with ID {request.TeacherId} not found."
            );

        // 2. Get roles
        IList<string> roles = await userManager.GetRolesAsync(teacher);

        // 3. Get lessons and VR learning sessions
        ReadOnlyCollection<AuthorizedGetTeacherAccountByIdLessonResponseDto> lessonDtos = teacher.Lessons
            .Select(lesson => new AuthorizedGetTeacherAccountByIdLessonResponseDto(
                lesson.Id,
                lesson.Name,
                lesson.Status.ToString()
            ))
            .ToList()
            .AsReadOnly();

        ReadOnlyCollection<AuthorizedGetTeacherAccountByIdVRLearningSessionResponseDto> sessionDtos = teacher.VRLearningSessions
            .Select(session => new AuthorizedGetTeacherAccountByIdVRLearningSessionResponseDto(
                session.Id,
                session.ClassName,
                session.CreatedAtUtc
            ))
            .ToList()
            .AsReadOnly();

        return new AuthorizedGetTeacherAccountByIdQueryResponseDto(
            TeacherId: teacher.Id,
            Email: teacher.Email ?? string.Empty,
            FullName: teacher.GetFullName(),
            Status: new StatusDto(teacher.Status),
            Roles: roles.AsReadOnly(),
            DateOfBirth: teacher.DateOfBirth,
            JoinedAtUtc: teacher.CreatedAtUtc,
            Lessons: lessonDtos,
            VRLearningSessions: sessionDtos
        );
    }
}
