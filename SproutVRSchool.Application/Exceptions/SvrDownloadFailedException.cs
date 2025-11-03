namespace SproutVRSchool.Application.Exceptions;

public sealed class SvrDownloadFailedException : Exception
{
    public SvrDownloadFailedException(string message) : base(message)
    {
    }
}
