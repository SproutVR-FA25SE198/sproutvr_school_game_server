using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningSession.V1;

namespace SproutVRSchool.Infrastructure.RoomServices;

public static class ServerToClientMessageFactory
{
    // ==============================================
    // === UpdateTaskSignal
    // ==============================================

    /// <summary>
    /// A method to create a server-to-client message confirming receipt of a task update.
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    public static ServerToClientMessage CreateSuccessTaskUpdateConfirmation(string taskId)
    {
        var successTaskUpdateConfirmation = new UpdateTaskSignal
        {
            Message = $"Task update for Task ID '{taskId}' received successfully.",
            Status = UpdateTaskSignal.Types.TaskStatus.Success
        };

        return new ServerToClientMessage { TaskSignal = successTaskUpdateConfirmation };
    }

    /// <summary>
    /// Creates a message indicating that a task update operation has failed for the specified task.
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    public static ServerToClientMessage CreateUpdateFailedTaskUpdateConfirmation()
    {
        var successTaskUpdateConfirmation = new UpdateTaskSignal
        {
            Message = $"Task update failed. Please try again.",
            Status = UpdateTaskSignal.Types.TaskStatus.UpdateFailed
        };

        return new ServerToClientMessage { TaskSignal = successTaskUpdateConfirmation };
    }

    /// <summary>
    /// Creates a server-to-client message indicating that a task update has failed due to an unexpected server error.
    /// </summary>
    /// <returns></returns>
    public static ServerToClientMessage CreateServerErrorTaskUpdateConfirmation()
    {
        var successTaskUpdateConfirmation = new UpdateTaskSignal
        {
            Message = $"Unexpected Server Error. Please try again",
            Status = UpdateTaskSignal.Types.TaskStatus.Unspecified
        };

        return new ServerToClientMessage { TaskSignal = successTaskUpdateConfirmation };
    }

    // ==============================================
    // === NotificationSignal
    // ==============================================

    /// <summary>
    /// Creates a generic notification message.
    /// </summary>
    public static ServerToClientMessage CreateInfoNotification(string text)
    {
        var notification = new NotificationSignal
        {
            Text = text,
            Severity = NotificationSignal.Types.Severity.Info
        };

        return new ServerToClientMessage { Notification = notification };
    }

    /// <summary>
    /// Creates a generic warning message.
    /// </summary>
    public static ServerToClientMessage CreateWarningNotification(string text)
    {
        var notification = new NotificationSignal
        {
            Text = text,
            Severity = NotificationSignal.Types.Severity.Warning
        };

        return new ServerToClientMessage { Notification = notification };
    }

    // ==============================================
    // === EndSessionSignal
    // ==============================================

    /// <summary>
    /// Creates an end session signal.
    /// </summary>
    /// <param name="reason"></param>
    /// <returns></returns>
    public static ServerToClientMessage CreateEndSessionSignal(DateTimeOffset dateTimeOffset)
    {
        var endSignal = new EndSessionSignal
        {
            Reason = $"The session has ended at {dateTimeOffset}. Please remove your VR devices and return to your seats.",
        };

        return new ServerToClientMessage { EndSignal = endSignal };
    }
}
