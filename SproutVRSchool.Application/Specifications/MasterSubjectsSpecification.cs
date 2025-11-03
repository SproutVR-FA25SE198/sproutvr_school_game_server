using SproutVRSchool.Application.RequestHandlers.Authorized.MasterSubjects.Queries.SearchMasterSubjects;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.MasterSubjects;

namespace SproutVRSchool.Application.Specifications;

public sealed class MasterSubjectsSpecification : BaseSpecification<MasterSubject>
{
    public MasterSubjectsSpecification(AuthorizedSearchMasterSubjectsQueryParams searchMasterSubjectsParam)
       : base(x =>
           (string.IsNullOrEmpty(searchMasterSubjectsParam.Name) || x.Name.Contains(searchMasterSubjectsParam.Name)) &&
           (!searchMasterSubjectsParam.MasterSubjectStatus.HasValue || x.Status == searchMasterSubjectsParam.MasterSubjectStatus))
    {
        // If pagination is true, apply pagination
        if (searchMasterSubjectsParam.IsPaginated!.Value && searchMasterSubjectsParam.PageSize.HasValue && searchMasterSubjectsParam.PageIndex.HasValue)
        {
            ApplyPaging(searchMasterSubjectsParam.PageSize.Value * (searchMasterSubjectsParam.PageIndex.Value - 1), searchMasterSubjectsParam.PageSize.Value);
        }

        // Sorting
        if (string.IsNullOrEmpty(searchMasterSubjectsParam.SortBy))
        {
            searchMasterSubjectsParam.SortBy = AppCts.SortingKeys.DEFAULT;
        }

        switch (searchMasterSubjectsParam.SortBy)
        {
            case AppCts.SortingKeys.MasterSubjects.NAME_ASC:
                AddOrderBy(x => x.Name);
                break;
            case AppCts.SortingKeys.MasterSubjects.NAME_DESC:
                AddOrderByDescending(x => x.Name);
                break;
            case AppCts.SortingKeys.CREATED_AT_UTC_ASC:
                AddOrderBy(x => x.CreatedAtUtc);
                break;
            case AppCts.SortingKeys.CREATED_AT_UTC_DESC:
                AddOrderByDescending(x => x.CreatedAtUtc);
                break;
            case AppCts.SortingKeys.UPDATED_AT_UTC_ASC:
                AddOrderBy(x => x.UpdatedAtUtc);
                break;
            case AppCts.SortingKeys.UPDATED_AT_UTC_DESC:
                AddOrderByDescending(x => x.UpdatedAtUtc);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }
    }
}
