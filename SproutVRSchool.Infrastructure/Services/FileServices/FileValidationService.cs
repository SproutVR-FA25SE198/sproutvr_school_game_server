using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SproutVRSchool.Application.Abstractions.FileServices;

namespace SproutVRSchool.Infrastructure.Services.FileServices;

public sealed class FileValidationService : IFileValidationService
{
    public bool IsExcelFile(IFormFile file)
    {
        if (!file.FileName.EndsWith(".xls", StringComparison.OrdinalIgnoreCase) &&
            !file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return true;
    }

    public bool IsFileValid(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return false;
        }

        return true;
    }

    public bool IsPdfFile(IFormFile file)
    {
        if (!file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return true;
    }
}
