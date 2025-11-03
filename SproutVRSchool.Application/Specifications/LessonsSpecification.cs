using SproutVRSchool.Application.RequestHandlers.Authorized.Lessons.Queries.SearchLessons;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Lessons;

namespace SproutVRSchool.Application.Specifications;

public sealed class LessonsSpecification : BaseSpecification<Lesson>
{
    /// <summary>
    /// Get the lesson by id
    /// </summary>
    /// <param name="id"></param>
    public LessonsSpecification(Guid id) : base(x => x.Id == id)
    {
        AddInclude(x => x.Subject);
        AddInclude(x => x.Teacher);
    }

    /// <summary>
    /// Filtering Lessons
    /// </summary>
    /// <param name="searchParams"></param>
    public LessonsSpecification(AuthorizedSearchLessonsQueryParams searchParams)
       : base(x =>
           (string.IsNullOrEmpty(searchParams.Name) || x.Name.Contains(searchParams.Name)) &&
           (!searchParams.LessonStatus.HasValue || x.Status == searchParams.LessonStatus) &&
           (!searchParams.SubjectId.HasValue || x.SubjectId == searchParams.SubjectId.Value) &&
           (!searchParams.TeacherId.HasValue || x.TeacherId == searchParams.TeacherId.Value))
    {
        // Pagination
        if (searchParams.IsPaginated!.Value && searchParams.PageSize.HasValue && searchParams.PageIndex.HasValue)
        {
            ApplyPaging(searchParams.PageSize.Value * (searchParams.PageIndex.Value - 1), searchParams.PageSize.Value);
        }

        // Sorting
        if (string.IsNullOrEmpty(searchParams.SortBy))
        {
            searchParams.SortBy = AppCts.SortingKeys.DEFAULT;
        }

        switch (searchParams.SortBy)
        {
            case AppCts.SortingKeys.Lessons.NAME_ASC:
                AddOrderBy(x => x.Name);
                break;
            case AppCts.SortingKeys.Lessons.NAME_DESC:
                AddOrderByDescending(x => x.Name);
                break;
            case AppCts.SortingKeys.CREATED_AT_UTC_ASC:
                AddOrderBy(x => x.CreatedAtUtc);
                break;
            case AppCts.SortingKeys.CREATED_AT_UTC_DESC:
                AddOrderByDescending(x => x.CreatedAtUtc);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }

        // Include navigation properties
        AddInclude(x => x.Subject);
        AddInclude(x => x.Teacher);
    }
}
