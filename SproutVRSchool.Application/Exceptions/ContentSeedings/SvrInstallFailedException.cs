namespace SproutVRSchool.Application.Exceptions.ContentSeedings;

public sealed class SvrInstallFailedException : Exception
{
    public SvrInstallFailedException(string message) : base(message)
    {
    }
}
