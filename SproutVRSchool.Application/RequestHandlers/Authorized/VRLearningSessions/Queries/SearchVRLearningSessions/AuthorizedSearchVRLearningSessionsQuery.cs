using MediatR;
using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRLearningSessions.Queries.SearchVRLearningSessions;

public sealed record AuthorizedSearchVRLearningSessionsQuery(
    AuthorizedSearchVRLearningSessionsQueryParams Params
) : IRequest<GetListResultResponseDto<AuthorizedSearchVRLearningSessionsQueryResponseDto>>;
