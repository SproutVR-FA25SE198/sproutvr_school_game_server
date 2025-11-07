using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.MasterSubjects;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.MasterSubjects.Queries.GetMasterSubjectById;

public sealed class AuthorizedGetMasterSubjectByIdQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
    ) : IRequestHandler<AuthorizedGetMasterSubjectByIdQuery, AuthorizedGetMasterSubjectByIdQueryResponseDto>
{
    public async Task<AuthorizedGetMasterSubjectByIdQueryResponseDto> Handle(AuthorizedGetMasterSubjectByIdQuery request, CancellationToken cancellationToken)
    {
        // 1. Fetch the MasterSubject entity from the repo
        var spec = new MasterSubjectsSpecification(request.Id);

        MasterSubject? masterSubject = await uow.Repository<MasterSubject>()
            .GetEntityBySpec(spec);

        // 2. If not found, throw SvrNotFoundException
        if (masterSubject is null)
        {
            throw new SvrResourceNotFoundException($"MasterSubject with ID {request.Id} not found.");
        }

        // 3. Map the entity to the response DTO
        return new AuthorizedGetMasterSubjectByIdQueryResponseDto
        {
            Id = masterSubject.Id,
            Name = masterSubject.Name,
            Description = masterSubject.Description,
            ImageUrl = masterSubject.ImageUrl,
            Status = new StatusDto(masterSubject.Status),
            CreatedAtUtc = masterSubject.CreatedAtUtc,
            CreatedAtVietNam = dateTimeProvider.ConvertToVietNamTime(masterSubject.CreatedAtUtc)
        };
    }
}
