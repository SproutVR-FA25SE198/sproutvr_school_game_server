using Google.Protobuf.WellKnownTypes;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Application.Commons.Requests;

/// <summary>
/// Base paging params for other specification params classes
/// </summary>
public class BaseGetListParams
{
    // ===========================
    // === Fields
    // ===========================

    public int? PageSize { get; set; }
    public int? PageIndex { get; set; }
    public bool? IsPaginated { get; set; } = AppCts.Api.DEFAULT_IS_PAGINATED;

    // ===========================
    // === Methods
    // ===========================

    public void ApplyPagingDefaults()
    {
        // If true value
        if (IsPaginated != null && IsPaginated.HasValue && IsPaginated.Value)
        {
            // Apply PageSize logic
            PageSize = PageSize.HasValue && PageSize.Value > AppCts.Api.DEFAULT_PAGE_SIZE
                       ? PageSize.Value
                       : AppCts.Api.DEFAULT_PAGE_SIZE;

            // Apply PageIndex logic
            PageIndex = PageIndex.HasValue && PageIndex.Value > AppCts.Api.DEFAULT_PAGE_INDEX
                        ? PageIndex.Value
                        : AppCts.Api.DEFAULT_PAGE_INDEX;
        }

        // if null or false value
        else if (IsPaginated == null || (IsPaginated.HasValue && !IsPaginated.Value))
        {
            PageSize = null;
            PageIndex = null;
        }
    }
}
