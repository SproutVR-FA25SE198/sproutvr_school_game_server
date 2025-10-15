using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SproutVRSchool.Application.Abstractions.Services.CodeGenerator;

public interface ICodeGeneratorService
{
    string GenerateCode(int length = 6);
}
