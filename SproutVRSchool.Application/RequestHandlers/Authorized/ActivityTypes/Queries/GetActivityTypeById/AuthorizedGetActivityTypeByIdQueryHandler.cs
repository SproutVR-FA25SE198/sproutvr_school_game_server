using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.ActivityTypes;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.ActivityTypes.Queries.GetActivityTypeById;

public sealed class AuthorizedGetActivityTypeByIdQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
    ) : IRequestHandler<AuthorizedGetActivityTypeByIdQuery, AuthorizedGetActivityTypeByIdQueryResponseDto>
{
    public async Task<AuthorizedGetActivityTypeByIdQueryResponseDto> Handle(AuthorizedGetActivityTypeByIdQuery request, CancellationToken cancellationToken)
    {
        // 1. Fetch the activity type entity from the repo
        var spec = new ActivityTypesSpecification(request.Id);

        ActivityType? activityType = await uow.Repository<ActivityType>()
            .GetEntityBySpec(spec);

        // 2. If not found, throw SvrNotFoundException
        if (activityType is null)
        {
            throw new SvrResourceNotFoundException($"ActivityType with ID {request.Id} not found.");
        }

        // 3. Map the activity type entity to AuthorizedGetActivityTypeByIdQueryResponseDto
        // 4. Return the response DTO
        return new AuthorizedGetActivityTypeByIdQueryResponseDto
        {
            Id = activityType.Id,
            Name = activityType.Name,
            ActivityCode = activityType.ActivityCode,
            CreatedAtUtc = activityType.CreatedAtUtc,
            CreatedAtVietNam = dateTimeProvider.ConvertToVietNamTime(activityType.CreatedAtUtc)
        };
    }
}
