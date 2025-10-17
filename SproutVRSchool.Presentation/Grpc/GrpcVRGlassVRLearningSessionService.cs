using Grpc.Core;
using LearningSession.V1;
using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession;
using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;

namespace SproutVRSchool.Presentation.Grpc;

public sealed class GrpcVRGlassVRLearningSessionService : VRGlassSessionManagement.VRGlassSessionManagementBase
{
    // ===============================
    // === Fields
    // ===============================

    private readonly IVRLearningSessionWithVRGlassService _vrLearningSessionWithVRGlassService;

    // ==============================
    // === Constructors
    // ==============================

    public GrpcVRGlassVRLearningSessionService(IVRLearningSessionWithVRGlassService vrLearningSessionWithVRGlassService)
    {
        _vrLearningSessionWithVRGlassService = vrLearningSessionWithVRGlassService;
    }

    // ==============================
    // === Methods
    // ==============================

    public override async Task<JoinRoomResponse> JoinRoom(JoinRoomRequest request, ServerCallContext context)
    {
        var joinRoomRequestDto = JoinRoomRequestDto.MapFromGrpcRequest(request);
        JoinRoomResponseDto resultDto = await _vrLearningSessionWithVRGlassService.JoinRoomAsync(joinRoomRequestDto);
        return JoinRoomResponseDto.MapToGrpcResponse(resultDto);
    }
}
