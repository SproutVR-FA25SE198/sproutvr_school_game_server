using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.Subjects;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Subjects.Queries.GetSubjectById;

public sealed class AuthorizedGetSubjectByIdQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
    ) : IRequestHandler<AuthorizedGetSubjectByIdQuery, AuthorizedGetSubjectByIdQueryResponseDto>
{
    public async Task<AuthorizedGetSubjectByIdQueryResponseDto> Handle(AuthorizedGetSubjectByIdQuery request, CancellationToken cancellationToken)
    {
        // 1. Fetch the subject entity from the repo, including MasterSubject
        var spec = new SubjectsSpecification(request.Id);

        Subject? subject = await uow.Repository<Subject>()
            .GetEntityBySpec(spec);

        // 2. If not found, throw SvrNotFoundException
        if (subject is null)
        {
            throw new SvrResourceNotFoundException($"Subject with ID {request.Id} not found.");
        }

        // 3. Map the MasterSubject info
        var masterSubject = new AuthorizedGetSubjectByIdQueryMasterSubjectResponseDto
        {
            Id = subject.MasterSubject.Id,
            Name = subject.MasterSubject.Name,
            Description = subject.MasterSubject.Description,
            ImageUrl = subject.MasterSubject.ImageUrl
        };

        // 4. Return the response DTO
        return new AuthorizedGetSubjectByIdQueryResponseDto
        {
            Id = subject.Id,
            MasterSubject = masterSubject,
            Name = subject.Name,
            Description = subject.Description,
            ImageUrl = subject.ImageUrl,
            Status = new StatusDto(subject.Status),
            CreatedAtUtc = subject.CreatedAtUtc,
            CreatedAtVietNam = dateTimeProvider.ConvertToVietNamTime(subject.CreatedAtUtc)
        };
    }
}
