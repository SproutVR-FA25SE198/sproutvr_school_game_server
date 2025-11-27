using LearningSession.V1;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession.Dtos;

public record AssignedDeviceRequestDto(string VrDeviceSerialNumber, string StudentName)
{
    public static AssignedDeviceRequestDto MapFromGrpcRequest(AssignedDevice assignedDevice)
    {
        return new AssignedDeviceRequestDto(
            assignedDevice.VrDeviceSerialNumber,
            assignedDevice.StudentName);
    }
}
