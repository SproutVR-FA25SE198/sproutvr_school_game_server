using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRDeviceSessionSummaries.Queries.GetVRDeviceSessionSummary;

public record AuthorizedGetVRDeviceSessionSummaryQuery(Guid VRLearningSessionId, Guid VRDeviceId)
    : IRequest<AuthorizedGetVRDeviceSessionSummaryQueryResponseDto>
{
}
