using SproutVRSchool.Application.Commons.Requests;
using SproutVRSchool.Domain.Entities.Identities;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Accounts.Queries.SearchAccounts;

public sealed class AuthorizedSearchAccountsQueryParams : BaseGetListParams
{
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public UserAccountStatus? Status { get; set; }
    public string? Role { get; set; }
    public string? SortBy { get; set; } = string.Empty;
}

