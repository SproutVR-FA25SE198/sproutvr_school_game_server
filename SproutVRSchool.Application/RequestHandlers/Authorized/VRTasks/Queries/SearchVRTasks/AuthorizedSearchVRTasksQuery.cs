using MediatR;
using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRTasks.Queries.SearchVRTasks;

public sealed record AuthorizedSearchVRTasksQuery(AuthorizedSearchVRTasksQueryParams AuthorizedSearchVRTasksQueryParams)
    : IRequest<GetListResultResponseDto<AuthorizedSearchVRTasksQueryResponseDto>>
{
}
