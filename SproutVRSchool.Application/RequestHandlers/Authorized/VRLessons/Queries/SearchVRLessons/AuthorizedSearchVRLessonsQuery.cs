using MediatR;
using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRLessons.Queries.SearchVRLessons;

public sealed record AuthorizedSearchVRLessonsQuery(AuthorizedSearchVRLessonsQueryParams AuthorizedSearchVRLessonsQueryParams)
    : IRequest<GetListResultResponseDto<AuthorizedSearchVRLessonsQueryResponseDto>>
{
}


