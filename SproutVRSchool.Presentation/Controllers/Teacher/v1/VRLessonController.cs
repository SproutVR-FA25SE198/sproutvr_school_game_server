using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Teacher.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/teacher/vrlessons")]
public class VRLessonController : BaseApiController
{

    // ========================
    // === POSTs
    // ========================

    // POST: api/v1/teacher/vrlessons

}
