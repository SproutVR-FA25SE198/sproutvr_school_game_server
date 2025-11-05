using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SproutVRSchool.Application.Abstractions.FileServices.Dtos;

namespace SproutVRSchool.Application.Abstractions.FileServices;

public interface IExcelFileService
{
    IReadOnlyList<TeacherAccountExcelRowDto> ReadTeacherExcel(IFormFile excelFile);
}
