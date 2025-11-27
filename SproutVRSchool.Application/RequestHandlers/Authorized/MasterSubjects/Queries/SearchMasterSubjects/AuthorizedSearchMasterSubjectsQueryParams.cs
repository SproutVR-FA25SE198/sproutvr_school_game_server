using SproutVRSchool.Application.Commons.Requests;
using SproutVRSchool.Domain.Entities.MasterSubjects;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.MasterSubjects.Queries.SearchMasterSubjects;

public class AuthorizedSearchMasterSubjectsQueryParams : BaseGetListParams
{
    public string? Name { get; set; }
    public MasterSubjectStatus? MasterSubjectStatus { get; set; }
    public string? SortBy { get; set; } = string.Empty;
}
