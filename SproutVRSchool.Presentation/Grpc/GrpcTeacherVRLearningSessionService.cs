using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using LearningSession.V1;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession.Dtos;

namespace SproutVRSchool.Presentation.Grpc;

public sealed class GrpcTeacherVRLearningSessionService : TeacherSessionManagement.TeacherSessionManagementBase
{

    // ===============================
    // === Fields
    // ===============================

    private readonly ITeacherVRLearningSessionService _vrLearningSessionTeacherService;

    // ===============================
    // === Constructors
    // ===============================

    public GrpcTeacherVRLearningSessionService(ITeacherVRLearningSessionService vrLearningSessionTeacherService)
    {
        _vrLearningSessionTeacherService = vrLearningSessionTeacherService;
    }

    // ===============================
    // === Methods
    // ===============================

    /// <summary>
    /// A function to activate the room
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<ActivateRoomResponse> ActivateRoom(ActivateRoomRequest request, ServerCallContext context)
    {
        var requestDto = ActivateRoomRequestDto.MapFromGrpcRequest(request);
        ActivateRoomResponseDto resultDto = await _vrLearningSessionTeacherService.ActivateRoomAsync(requestDto);

        return ActivateRoomResponseDto.MapToGrpcResponse(resultDto);
    }

    /// <summary>
    /// Cancels a VR learning session room
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<CancelRoomResponse> CancelRoom(CancelRoomRequest request, ServerCallContext context)
    {
        string vrLearningSessionId = request.VrLearningSessionId;
        CancelRoomResponseDto resultDto = await _vrLearningSessionTeacherService.CancelRoomAsync(vrLearningSessionId);

        return CancelRoomResponseDto.MapToGrpcResponse(resultDto);
    }

    /// <summary>
    /// A function to create a VR learning session room
    /// </summary>
    /// <param name="request"></param>   
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<CreateRoomResponse> CreateRoom(CreateRoomRequest request, ServerCallContext context)
    {
        var requestDto = CreateRoomRequestDto.MapFromGrpcRequest(request);

        CreateRoomResponseDto resultDto = await _vrLearningSessionTeacherService.CreateRoomAsync(requestDto);

        return CreateRoomResponseDto.MapToGrpcResponse(resultDto);
    }

    /// <summary>
    /// A function to send notification to all participants in the VR learning session
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<Empty> SendNotification(SendNotificationRequest request, ServerCallContext context)
    {
        var requestDto = SendNotificationRequestDto.MapFromGrpcRequest(request);
        await _vrLearningSessionTeacherService.SendNotificationAsync(requestDto);
        return new Empty();
    }
}
