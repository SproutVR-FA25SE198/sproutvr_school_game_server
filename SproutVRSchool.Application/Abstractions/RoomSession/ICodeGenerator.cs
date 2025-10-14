using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SproutVRSchool.Application.Abstractions.RoomSession;

public interface ICodeGenerator
{
    string GenerateCode(int length = 6);
}
