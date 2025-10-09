using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Helpers.Responses;

namespace SproutVRSchool.Presentation.Areas;

[Route("api/[controller]")]
[ApiController]
internal sealed class BaseApiController : ControllerBase
{
    internal ActionResult PaginationOkResult<T>(IReadOnlyList<T> items, int count, int pageIndex, int pageSize)
    {
        var pagination = new PaginatedResult<T>(pageIndex, pageSize, count, items);
        return Ok(pagination);
    }
}
