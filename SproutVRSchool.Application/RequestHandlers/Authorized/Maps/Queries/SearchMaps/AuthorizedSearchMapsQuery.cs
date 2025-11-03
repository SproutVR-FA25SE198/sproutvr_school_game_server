using MediatR;
using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Maps.Queries.SearchMaps;

public record AuthorizedSearchMapsQuery(AuthorizedSearchMapsQueryParams SearchMapsParams)
    : IRequest<GetListResultResponseDto<AuthorizedSearchMapsQueryResponseDto>>
{
}
