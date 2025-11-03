using MediatR;
using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.MasterSubjects.Queries.SearchMasterSubjects;

public sealed record AuthorizedSearchMasterSubjectsQuery(AuthorizedSearchMasterSubjectsQueryParams AuthorizedSearchMasterSubjectsQueryParams)
    : IRequest<GetListResultResponseDto<AuthorizedSearchMasterSubjectsQueryResponseDto>>
{
}
