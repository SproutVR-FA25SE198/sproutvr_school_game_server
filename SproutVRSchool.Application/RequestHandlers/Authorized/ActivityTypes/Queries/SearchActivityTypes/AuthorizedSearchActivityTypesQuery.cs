using MediatR;
using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.ActivityTypes.Queries.SearchActivityTypes;

public sealed record AuthorizedSearchActivityTypesQuery(
    AuthorizedSearchActivityTypesQueryParams AuthorizedSearchActivityTypesQueryParams
) : IRequest<GetListResultResponseDto<AuthorizedSearchActivityTypesQueryResponseDto>>;
