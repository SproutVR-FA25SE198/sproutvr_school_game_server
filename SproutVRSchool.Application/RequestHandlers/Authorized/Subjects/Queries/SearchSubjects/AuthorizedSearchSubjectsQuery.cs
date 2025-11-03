using MediatR;
using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Subjects.Queries.SearchSubjects;

public sealed record AuthorizedSearchSubjectsQuery(AuthorizedSearchSubjectsQueryParams AuthorizedSearchSubjectsQueryParams)
    : IRequest<GetListResultResponseDto<AuthorizedSearchSubjectsQueryResponseDto>>
{
}
