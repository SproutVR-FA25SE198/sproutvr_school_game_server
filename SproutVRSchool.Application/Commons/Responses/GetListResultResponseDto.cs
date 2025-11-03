using System.Collections.Generic;
using System.Text.Json.Serialization;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Application.Commons.Responses;

public record GetListResultResponseDto<T>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? PageIndex { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? PageSize { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TotalPages { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TotalItemsPerPage { get; init; }

    public int TotalItems { get; init; }

    public IReadOnlyList<T> Items { get; init; }

    public GetListResultResponseDto(
        int? pageIndex,
        int? pageSize,
        int totalItems,
        IReadOnlyList<T> items)
    {
        PageIndex = pageIndex ?? null;

        PageSize = pageSize ?? null;

        TotalItems = totalItems;

        TotalPages = (pageIndex.HasValue && pageSize.HasValue && pageSize.Value > 0)
            ? (int)Math.Ceiling((double)TotalItems / pageSize.Value)
            : null;

        Items = items;

        TotalItemsPerPage = pageIndex.HasValue && pageSize.HasValue
            ? items.Count
            : null;
    }
}
