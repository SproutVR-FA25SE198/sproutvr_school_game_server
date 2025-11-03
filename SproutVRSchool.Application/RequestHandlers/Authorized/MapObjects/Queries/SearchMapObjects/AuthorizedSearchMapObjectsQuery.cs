using MediatR;
using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.MapObjects.Queries.SearchMapObjects;

public sealed record AuthorizedSearchMapObjectsQuery(
    AuthorizedSearchMapObjectsQueryParams AuthorizedSearchMapObjectsQueryParams
) : IRequest<GetListResultResponseDto<AuthorizedSearchMapObjectsQueryResponseDto>>;

