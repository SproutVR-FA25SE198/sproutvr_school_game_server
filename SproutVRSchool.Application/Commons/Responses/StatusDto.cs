using System.Globalization;

namespace SproutVRSchool.Application.Commons.Responses;

public class StatusDto
{
    public int Key { get; init; }
    public string Name { get; init; }

    public StatusDto(Enum status)
    {
        Key = Convert.ToInt32(status, CultureInfo.InvariantCulture);
        Name = status.ToString();
    }
}
