using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Identities;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Accounts.Queries.SearchAccounts;

public sealed class AuthorizedSearchAccountsQueryHandler(
    UserManager<UserAccount> userManager,
    IDateTimeProvider dateTimeProvider
) : IRequestHandler<AuthorizedSearchAccountsQuery, GetListResultResponseDto<AuthorizedSearchAccountsQueryResponseDto>>
{
    /*
        Handle Pagination manually due to Identity DbContext is on top of EF Core DbContext, don't use uow
     */
    public async Task<GetListResultResponseDto<AuthorizedSearchAccountsQueryResponseDto>> Handle(
    AuthorizedSearchAccountsQuery request,
    CancellationToken cancellationToken)
    {
        // 1. Prepare query parameters
        AuthorizedSearchAccountsQueryParams searchParams = request.AuthorizedSearchAccountsQueryParams;
        IQueryable<UserAccount> query = userManager.Users.AsNoTracking();

        // 2. Filtering first
        // Email
        if (!string.IsNullOrWhiteSpace(searchParams.Email))
        {
            query = query.Where(u => u.Email != null && u.Email.Contains(searchParams.Email));
        }

        // First Name
        if (!string.IsNullOrWhiteSpace(searchParams.FirstName))
        {
            query = query.Where(u => u.FirstName.Contains(searchParams.FirstName));
        }

        // Last Name
        if (!string.IsNullOrWhiteSpace(searchParams.LastName))
        {
            query = query.Where(u => u.LastName.Contains(searchParams.LastName));
        }

        // Status
        if (searchParams.Status != null)
        {
            query = query.Where(u => u.Status == searchParams.Status);
        }

        // Role
        if (!string.IsNullOrWhiteSpace(searchParams.Role))
        {
            IList<UserAccount> usersInRole = await userManager.GetUsersInRoleAsync(searchParams.Role);
            var userIdsInRole = usersInRole.Select(u => u.Id).ToHashSet();
            query = query.Where(u => userIdsInRole.Contains(u.Id));
        }

        // 3. Sorting
        if (string.IsNullOrEmpty(searchParams.SortBy))
        {
            searchParams.SortBy = AppCts.SortingKeys.DEFAULT;
        }

        query = searchParams.SortBy switch
        {
            AppCts.SortingKeys.UserAccounts.FIRST_NAME_ASC => query.OrderBy(x => x.FirstName),
            AppCts.SortingKeys.UserAccounts.FIRST_NAME_DESC => query.OrderByDescending(x => x.FirstName),
            AppCts.SortingKeys.UserAccounts.LAST_NAME_ASC => query.OrderBy(x => x.LastName),
            AppCts.SortingKeys.UserAccounts.LAST_NAME_DESC => query.OrderByDescending(x => x.LastName),
            AppCts.SortingKeys.UserAccounts.EMAIL_ASC => query.OrderBy(x => x.Email),
            AppCts.SortingKeys.UserAccounts.EMAIL_DESC => query.OrderByDescending(x => x.Email),
            AppCts.SortingKeys.UserAccounts.STATUS_ASC => query.OrderBy(x => x.Status),
            AppCts.SortingKeys.UserAccounts.STATUS_DESC => query.OrderByDescending(x => x.Status),
            AppCts.SortingKeys.CREATED_AT_UTC_ASC => query.OrderBy(x => x.CreatedAtUtc),
            AppCts.SortingKeys.CREATED_AT_UTC_DESC => query.OrderByDescending(x => x.CreatedAtUtc),
            AppCts.SortingKeys.UPDATED_AT_UTC_ASC => query.OrderBy(x => x.UpdatedAtUtc),
            AppCts.SortingKeys.UPDATED_AT_UTC_DESC => query.OrderByDescending(x => x.UpdatedAtUtc),
            _ => query.OrderByDescending(x => x.CreatedAtUtc),
        };

        // 4. TotalItems & Items
        int totalItems = await query.CountAsync(cancellationToken);
        List<UserAccount> users = await query.ToListAsync(cancellationToken);

        // 5. Pagination
        if (searchParams.IsPaginated!.Value && searchParams.PageSize.HasValue && searchParams.PageIndex.HasValue)
        {
            int pageIndex = searchParams.PageIndex.Value;
            int pageSize = searchParams.PageSize.Value;

            users = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        // Since we have the async function inside the Select, we need to use Task.WhenAll to await all tasks
        var items = new List<AuthorizedSearchAccountsQueryResponseDto>();
        foreach (UserAccount user in users)
        {
            // Await this call. The loop will pause here until the roles are fetched.
            IList<string> roles = await userManager.GetRolesAsync(user) ?? Array.Empty<string>();

            var dto = new AuthorizedSearchAccountsQueryResponseDto(
                UserId: user.Id,
                FullName: user.GetFullName(),
                Roles: roles.AsReadOnly(),
                Email: user.Email ?? string.Empty,
                Status: user.Status.ToString(),
                DateOfBirth: user.DateOfBirth,
                CreatedAtUtc: user.CreatedAtUtc,
                CreatedAtVietNam: dateTimeProvider.ConvertToVietNamTime(user.CreatedAtUtc)
            );

            items.Add(dto);
        }

        // 6. Return response
        return new GetListResultResponseDto<AuthorizedSearchAccountsQueryResponseDto>(
            pageSize: searchParams.PageSize,
            pageIndex: searchParams.PageIndex,
            totalItems: totalItems,
            items: items
        );
    }
}

