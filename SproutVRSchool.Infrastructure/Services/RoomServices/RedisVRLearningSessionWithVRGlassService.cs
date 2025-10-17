using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage;
using SproutVRSchool.Application.Abstractions.RoomServices.SessionValidator;
using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession;
using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;

namespace SproutVRSchool.Infrastructure.Services.RoomServices;

public sealed class RedisVRLearningSessionWithVRGlassService : IVRLearningSessionWithVRGlassService
{
    // ===============================
    // === Fields
    // ===============================

    private readonly IDatabase _database;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IVRLearningSessionValidator _validator;

    // ===============================
    // === Constructors
    // ===============================
    public RedisVRLearningSessionWithVRGlassService(IDatabase database, JsonSerializerOptions jsonOptions, IVRLearningSessionValidator validator)
    {
        _database = database;
        _jsonOptions = jsonOptions;
        _validator = validator;
    }

    // ===============================
    // === Methods
    // ===============================

    public Task<JoinRoomResponseDto> JoinRoomAsync(JoinRoomRequestDto joinRoomRequestDto)
    {
        throw new NotImplementedException();
    }

    public Task HandleTaskUpdateAsync(TaskUpdateDto request)
    {
        throw new NotImplementedException();
    }
}
