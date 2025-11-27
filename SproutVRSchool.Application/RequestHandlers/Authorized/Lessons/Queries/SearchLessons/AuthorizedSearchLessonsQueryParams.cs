using SproutVRSchool.Application.Commons.Requests;
using SproutVRSchool.Domain.Entities.Lessons;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Lessons.Queries.SearchLessons;

public class AuthorizedSearchLessonsQueryParams : BaseGetListParams
{
    public string? Name { get; set; }
    public LessonStatus? LessonStatus { get; set; }
    public Guid? SubjectId { get; set; }
    public Guid? TeacherId { get; set; }
    public Guid? MasterSubjectId { get; set; }
    public string? SortBy { get; set; } = string.Empty;
}
