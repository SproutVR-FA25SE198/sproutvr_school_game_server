using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.VRDeviceSessionSummaries;
using SproutVRSchool.Domain.Entities.VRDeviceTaskProgresses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRDeviceSessionSummaries.Queries.GetVRDeviceSessionSummary;

public class AuthorizedGetVRDeviceSessionSummaryQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
) : IRequestHandler<AuthorizedGetVRDeviceSessionSummaryQuery, AuthorizedGetVRDeviceSessionSummaryQueryResponseDto>
{
    public async Task<AuthorizedGetVRDeviceSessionSummaryQueryResponseDto> Handle(
        AuthorizedGetVRDeviceSessionSummaryQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Create the spec (it now includes VRLearningSession.VRDeviceTaskProgresses)
        var spec = new VRDeviceSessionSummariesSpecification(request.VRLearningSessionId, request.VRDeviceId);

        // 2. Get the entity
        VRDeviceSessionSummary summary = await uow.Repository<VRDeviceSessionSummary>().GetEntityBySpec(spec);

        // 3. Check if found
        if (summary == null)
        {
            throw new SvrResourceNotFoundException(
                $"VR Device Session Summary with VRDevice Id '{request.VRDeviceId}' " +
                $"and VR Learning Session Id '{request.VRLearningSessionId}' was not found.");
        }

        // Count Progesses Tasks belong to the VRDeviceTaskProgress
        ICollection<VRDeviceTaskProgress> allProgresses = summary.VRLearningSession?.VRDeviceTaskProgresses ?? [];
        var studentProgresses = allProgresses
            .Where(p => p.VRDeviceId == summary.VRDeviceId)
            .ToList();

        int totalTasks = studentProgresses.Count;
        int noTasksCompleted = studentProgresses.Count(p => p.IsCompleted);
        int noTasksUncompleted = totalTasks - noTasksCompleted;
        int noCorrected = studentProgresses.Count(p => p.IsCorrect);
        int noInCorrected = studentProgresses.Count(p => !p.IsCompleted || !p.IsCorrect);

        // ======================================================

        // 4. Map to DTO
        return new AuthorizedGetVRDeviceSessionSummaryQueryResponseDto(
            VRDeviceId: summary.VRDeviceId,
            VRLearningSessionId: summary.VRLearningSessionId,
            StudentName: summary.StudentName,

            // Pass in the new calculated values
            NoTasksCompleted: noTasksCompleted,
            NoTasksUncompleted: noTasksUncompleted,
            NoCorrected: noCorrected,
            NoInCorrected: noInCorrected,
            TotalTasks: totalTasks,

            // Nested VRDevice DTO
            VRDevice: new AuthorizedGetVRDeviceSessionSummaryQueryDeviceResponseDto(
                    summary.VRDeviceId,
                    summary.VRDevice.Name,
                    summary.VRDevice.SerialNumber
                ),

            // Nested VRLearningSession DTO
            VRLearningSession: new AuthorizedGetVRDeviceSessionSummaryQueryVRLearningSessionResponseDto(
                    summary.VRLearningSessionId,
                    summary.VRLearningSession!.ClassName
                ),

            CreatedAtUtc: summary.CreatedAtUtc,
            CreatedAtVietNam: dateTimeProvider.ConvertToVietNamTime(summary.CreatedAtUtc)
        );
    }
}
