using MediatR;
using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Accounts.Queries.SearchAccounts;

public sealed record AuthorizedSearchAccountsQuery(
    AuthorizedSearchAccountsQueryParams AuthorizedSearchAccountsQueryParams
) : IRequest<GetListResultResponseDto<AuthorizedSearchAccountsQueryResponseDto>>;

