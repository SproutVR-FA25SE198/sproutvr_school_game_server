using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Application.Abstractions.Services.CodeGenerator;

namespace SproutVRSchool.Infrastructure.Services;

public class CodeGenerator : ICodeGeneratorService
{
    private readonly string _chars = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";

    public string GenerateCode(int length = 6)
    {
        var code = new StringBuilder();

        for (int pos = 0; pos < length; pos++)
        {
            int randomIndex = RandomNumberGenerator.GetInt32(0, _chars.Length);
            code.Append(_chars[randomIndex]);
        }

        return code.ToString();
    }
}
