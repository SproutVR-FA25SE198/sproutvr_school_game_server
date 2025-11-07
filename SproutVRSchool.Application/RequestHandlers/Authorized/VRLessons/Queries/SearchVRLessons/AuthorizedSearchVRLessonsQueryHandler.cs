using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.VRLessons;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRLessons.Queries.SearchVRLessons;

public sealed class AuthorizedSearchVRLessonsQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
    ) : IRequestHandler<AuthorizedSearchVRLessonsQuery, GetListResultResponseDto<AuthorizedSearchVRLessonsQueryResponseDto>>
{
    public async Task<GetListResultResponseDto<AuthorizedSearchVRLessonsQueryResponseDto>> Handle(AuthorizedSearchVRLessonsQuery request, CancellationToken cancellationToken)
    {
        // 1. Get items & count from repository using the Specification
        (IReadOnlyList<VRLesson> Data, int Count) rawLists = await uow.Repository<VRLesson>()
            .ListAsync(new VRLessonsSpecification(request.AuthorizedSearchVRLessonsQueryParams));

        // 2. Map the entities to the response DTOs
        var mappedItems = rawLists.Data.Select(vrLesson => new AuthorizedSearchVRLessonsQueryResponseDto
        {
            Id = vrLesson.Id,
            Name = vrLesson.Name,
            Description = vrLesson.Description,
            MaxDuration = vrLesson.MaxDuration,
            PresetJsonRelativeFilePath = vrLesson.PresetJsonRelativeFilePath,
            Status = new StatusDto(vrLesson.Status),
            CreatedAtUtc = vrLesson.CreatedAtUtc,
            CreatedAtVietNam = dateTimeProvider.ConvertToVietNamTime(vrLesson.CreatedAtUtc),

            // Map navigation properties
            Lesson = new AuthorizedVRLessonLessonQueryResponseDto(
                Id: vrLesson.Lesson.Id,
                Name: vrLesson.Lesson.Name,
                Description: vrLesson.Lesson.Description
            ),
            Map = new AuthorizedVRLessonMapQueryResponseDto(
                Id: vrLesson.Map.Id,
                Name: vrLesson.Map.Name,
                MapCode: vrLesson.Map.MapCode
            )
        }).ToList();

        // 3. Construct the final paginated response DTO
        var result = new GetListResultResponseDto<AuthorizedSearchVRLessonsQueryResponseDto>(
            pageSize: request.AuthorizedSearchVRLessonsQueryParams.PageSize,
            pageIndex: request.AuthorizedSearchVRLessonsQueryParams.PageIndex,
            totalItems: rawLists.Count,
            items: mappedItems
        );

        return result;
    }
}
