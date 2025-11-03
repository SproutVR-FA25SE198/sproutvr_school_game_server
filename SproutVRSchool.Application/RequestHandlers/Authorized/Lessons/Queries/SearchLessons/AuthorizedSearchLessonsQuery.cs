using MediatR;
using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Lessons.Queries.SearchLessons;

public sealed record AuthorizedSearchLessonsQuery(AuthorizedSearchLessonsQueryParams AuthorizedSearchLessonsQueryParams)
    : IRequest<GetListResultResponseDto<AuthorizedSearchLessonsQueryResponseDto>>;
