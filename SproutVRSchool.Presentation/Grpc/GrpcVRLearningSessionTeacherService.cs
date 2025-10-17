using Grpc.Core;
using LearningSession.V1;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession.Dtos;

namespace SproutVRSchool.Presentation.Grpc;

public sealed class GrpcVRLearningSessionTeacherService : LearningSessionManagement.LearningSessionManagementBase
{

    // ===============================
    // === Fields
    // ===============================

    private readonly IVRLearningSessionTeacherService _vrLearningSessionTeacherService;

    // ===============================
    // === Constructors
    // ===============================
    public GrpcVRLearningSessionTeacherService(IVRLearningSessionTeacherService vrLearningSessionTeacherService)
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
}
