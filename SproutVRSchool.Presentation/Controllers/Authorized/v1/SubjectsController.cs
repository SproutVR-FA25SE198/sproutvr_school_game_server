using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Requests;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.Authorized.Maps.Querries.SearchMaps;
using SproutVRSchool.Application.RequestHandlers.Authorized.VRDevices.Querries.GetVRDevice;
using SproutVRSchool.Application.RequestHandlers.Authorized.VRDevices.Querries.SearchVRDevices;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.MasterSubjects;
using SproutVRSchool.Domain.Entities.Subjects;
using SproutVRSchool.Domain.Entities.VRDevices;

namespace SproutVRSchool.Presentation.Controllers.Authorized.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/authorized/maps")]
public class SubjectsController(IMediator mediator) : BaseApiController
{
    // ========================
    // === GETs
    // ========================

    // GET: api/v1/authorized/subjects

    [HttpGet]
    public async Task<ActionResult<GetListResultResponseDto<AuthorizedSearchSubjectsQueryResponseDto>>> SearchSubjects(
        [FromQuery] AuthorizedSearchSubjectsQueryParams @params,
        CancellationToken cancellationToken
    )
    {
        var query = new AuthorizedSearchSubjectsQuery(@params);
        GetListResultResponseDto<AuthorizedSearchSubjectsQueryResponseDto> result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }



    // ========================
    // === PUTs
    // ========================

    // ========================
    // === PATCHs
    // ========================
}

public sealed record AuthorizedSearchSubjectsQuery(AuthorizedSearchSubjectsQueryParams AuthorizedSearchSubjectsQueryParams)
    : IRequest<GetListResultResponseDto<AuthorizedSearchSubjectsQueryResponseDto>>
{
}


public class AuthorizedSearchSubjectsQueryParams : BaseGetListParams
{
    public Guid? MasterSubjectId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public SubjectStatus? SubjectStatus { get; set; }
    public string? SortBy { get; set; } = string.Empty;
}

public record AuthorizedSearchSubjectsQueryResponseDto(
    Guid Id,
    AuthorizedSearchSubjectsQueryMasterSubjectResponseDto MasterSubject,
    string Name,
    string Description,
    string ImageUrl,
    StatusDto Status,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietNam
);

public record AuthorizedSearchSubjectsQueryMasterSubjectResponseDto(
    string Name,
    string Description,
    string ImageUrl,
    StatusDto Status,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietNam
);

public class AuthorizedSearchSubjectsQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
) : IRequestHandler<AuthorizedSearchSubjectsQuery, GetListResultResponseDto<AuthorizedSearchSubjectsQueryResponseDto>>
{
    public async Task<GetListResultResponseDto<AuthorizedSearchSubjectsQueryResponseDto>> Handle(
        AuthorizedSearchSubjectsQuery request,
        CancellationToken cancellationToken)
    {
        // Get items & count from params
        (IReadOnlyList<Subject> Data, int Count) rawLists = await uow.Repository<Subject>()
            .ListAsync(new SubjectsSpecification(request.AuthorizedSearchSubjectsQueryParams));

        var items = rawLists.Data.Select(subject => new AuthorizedSearchSubjectsQueryResponseDto(
            Id: subject.Id,
            MasterSubject: new AuthorizedSearchSubjectsQueryMasterSubjectResponseDto(
                Name: subject.MasterSubject.Name,
                Description: subject.MasterSubject.Description,
                ImageUrl: subject.MasterSubject.ImageUrl,
                Status: new StatusDto(subject.MasterSubject.Status),
                CreatedAtUtc: subject.MasterSubject.CreatedAtUtc,
                CreatedAtVietNam: dateTimeProvider.ConvertToVietNamTime(subject.MasterSubject.CreatedAtUtc)
            ),
            Name: subject.Name,
            Description: subject.Description,
            ImageUrl: subject.ImageUrl,
            Status: new StatusDto(subject.Status),
            CreatedAtUtc: subject.CreatedAtUtc,
            CreatedAtVietNam: dateTimeProvider.ConvertToVietNamTime(subject.CreatedAtUtc)
        )).ToList();

        var result = new GetListResultResponseDto<AuthorizedSearchSubjectsQueryResponseDto>(
            pageSize: request.AuthorizedSearchSubjectsQueryParams.PageSize,
            pageIndex: request.AuthorizedSearchSubjectsQueryParams.PageIndex,
            totalItems: rawLists.Count,
            items: items
        );

        return result;
    }
}


public sealed class SubjectsSpecification : BaseSpecification<Subject>
{
    public SubjectsSpecification(AuthorizedSearchSubjectsQueryParams searchSubjectsParam)
       : base(x =>
           (string.IsNullOrEmpty(searchSubjectsParam.Name) || x.Name.Contains(searchSubjectsParam.Name)) &&
           (!searchSubjectsParam.MasterSubjectId.HasValue || x.MasterSubjectId == searchSubjectsParam.MasterSubjectId) &&
           (!searchSubjectsParam.SubjectStatus.HasValue || x.Status == searchSubjectsParam.SubjectStatus))
    {
        // If pagination is true, apply pagination
        if (searchSubjectsParam.IsPaginated!.Value && searchSubjectsParam.PageSize.HasValue && searchSubjectsParam.PageIndex.HasValue)
        {
            ApplyPaging(searchSubjectsParam.PageSize.Value * (searchSubjectsParam.PageIndex.Value - 1), searchSubjectsParam.PageSize.Value);
        }

        // Sorting
        if (string.IsNullOrEmpty(searchSubjectsParam.SortBy))
        {
            searchSubjectsParam.SortBy = AppCts.SortingKeys.DEFAULT;
        }

        switch (searchSubjectsParam.SortBy)
        {
            case AppCts.SortingKeys.Subjects.NAME_ASC:
                AddOrderBy(x => x.Name);
                break;
            case AppCts.SortingKeys.Subjects.NAME_DESC:
                AddOrderByDescending(x => x.Name);
                break;
            case AppCts.SortingKeys.CREATED_AT_UTC_ASC:
                AddOrderBy(x => x.CreatedAtUtc);
                break;
            case AppCts.SortingKeys.CREATED_AT_UTC_DESC:
                AddOrderByDescending(x => x.CreatedAtUtc);
                break;
            case AppCts.SortingKeys.UPDATED_AT_UTC_ASC:
                AddOrderBy(x => x.UpdatedAtUtc);
                break;
            case AppCts.SortingKeys.UPDATED_AT_UTC_DESC:
                AddOrderByDescending(x => x.UpdatedAtUtc);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }

        // Include
        AddInclude(x => x.MasterSubject);
    }
}
