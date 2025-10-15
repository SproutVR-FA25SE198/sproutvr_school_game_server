using System.Threading.Tasks;
using SproutVRSchool.Application.Abstractions.Services.TeacherSession.Dtos;

namespace SproutVRSchool.Application.Abstractions.Services.TeacherSession;

public interface IVRLearningSessionTeacherService
{
    Task<CreateRoomResponseDto> CreateRoomAsync(CreateRoomRequestDto request);

    Task<ActivateRoomResponseDto> ActivateRoomAsync(ActivateRoomRequestDto request);

    Task<CancelRoomResponseDto> CancelRoomAsync(string vrLearningSessionId);
}




