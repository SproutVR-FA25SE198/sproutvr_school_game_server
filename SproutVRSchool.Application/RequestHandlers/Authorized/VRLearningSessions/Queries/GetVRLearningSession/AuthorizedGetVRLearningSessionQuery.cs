using MediatR;
using SproutVRSchool.Application.RequestHandlers.Authorized.VRLearningSessions.Queries.SearchVRLearningSessions;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRLearningSessions.Queries.GetVRLearningSession;

public record AuthorizedGetVRLearningSessionQuery(
    Guid Id) : IRequest<AuthorizedGetVRLearningSessionQueryResponseDto>;
