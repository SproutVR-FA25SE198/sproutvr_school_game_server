using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities;

namespace SproutVRSchool.Application.Commons.Responses;

public record PaginatedResultDto<T>(
    int? pageIndex,
    int? pageSize,
    int? count,
    IReadOnlyList<T> data)
{
    int PageIndex { get; init; } = (pageIndex == null || pageIndex.HasValue && pageIndex.Value < 1)
        ? AppCts.Api.DEFAULT_PAGE_INDEX
        : pageIndex.Value;

    int PageSize { get; init; } = (pageSize == null || pageSize.HasValue && pageSize.Value < 1)
        ? AppCts.Api.DEFAULT_PAGE_SIZE
        : pageSize.Value;

    int Count { get; init; } = count.HasValue
        ? count.Value
        : AppCts.Api.DEFAULT_COUNT;

    int TotalPage { get; } = (count.HasValue && pageSize.HasValue && pageSize.Value > 0)
        ? (int)Math.Ceiling((double)count.Value / pageSize.Value)
        : AppCts.Api.DEFAULT_TOTAL_PAGES;

    IReadOnlyList<T> Data { get; init; } = data ?? new List<T>();
}
