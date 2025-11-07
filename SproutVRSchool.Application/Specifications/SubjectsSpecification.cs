using SproutVRSchool.Application.RequestHandlers.Authorized.Subjects.Queries.SearchSubjects;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Subjects;

namespace SproutVRSchool.Application.Specifications;

public sealed class SubjectsSpecification : BaseSpecification<Subject>
{
    public SubjectsSpecification(AuthorizedSearchSubjectsQueryParams searchSubjectsParam)
       : base(x =>
           (string.IsNullOrEmpty(searchSubjectsParam.Name) || x.Name.Contains(searchSubjectsParam.Name)) &&
           (!searchSubjectsParam.MasterSubjectId.HasValue || x.MasterSubjectId == searchSubjectsParam.MasterSubjectId) &&
           (!searchSubjectsParam.SubjectStatus.HasValue || x.Status == searchSubjectsParam.SubjectStatus))
    {
        // If pagination is true, apply pagination
        if (searchSubjectsParam.IsPaginated!.Value && searchSubjectsParam.PageSize.HasValue && searchSubjectsParam.PageIndex.HasValue)
        {
            ApplyPaging(searchSubjectsParam.PageSize.Value * (searchSubjectsParam.PageIndex.Value - 1), searchSubjectsParam.PageSize.Value);
        }

        // Sorting
        if (string.IsNullOrEmpty(searchSubjectsParam.SortBy))
        {
            searchSubjectsParam.SortBy = AppCts.SortingKeys.DEFAULT;
        }

        switch (searchSubjectsParam.SortBy)
        {
            case AppCts.SortingKeys.Subjects.NAME_ASC:
                AddOrderBy(x => x.Name);
                break;
            case AppCts.SortingKeys.Subjects.NAME_DESC:
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

        // Include
        AddInclude(x => x.MasterSubject);
    }

    public SubjectsSpecification(Guid id) : base(x => x.Id == id)
    {
        AddInclude(x => x.MasterSubject);
    }
}
