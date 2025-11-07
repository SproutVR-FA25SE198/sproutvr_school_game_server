using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.MasterSubjects.Queries.GetMasterSubjectById;

public record AuthorizedGetMasterSubjectByIdQuery(Guid Id)
    : IRequest<AuthorizedGetMasterSubjectByIdQueryResponseDto>
{
}
