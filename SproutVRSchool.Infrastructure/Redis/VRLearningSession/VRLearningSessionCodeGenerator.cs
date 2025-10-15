using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using SproutVRSchool.Application.Abstractions.RoomSession;

namespace SproutVRSchool.Infrastructure.Redis.VRLearningSession;

public class VRLearningSessionCodeGenerator : ICodeGenerator
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
