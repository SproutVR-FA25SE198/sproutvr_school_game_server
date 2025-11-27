namespace SproutVRSchool.Application.Exceptions.ContentSeedings;

public sealed class SvrDownloadFailedException : Exception
{
    public SvrDownloadFailedException(string message) : base(message)
    {
    }
}
