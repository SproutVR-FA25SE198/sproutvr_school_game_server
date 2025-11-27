using SproutVRSchool.Application.Commons.Requests;
using SproutVRSchool.Domain.Entities.Subjects;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Subjects.Queries.SearchSubjects;

public class AuthorizedSearchSubjectsQueryParams : BaseGetListParams
{
    public Guid? MasterSubjectId { get; set; }
    public string? Name { get; set; }
    public SubjectStatus? SubjectStatus { get; set; }
    public string? SortBy { get; set; } = string.Empty;
}
