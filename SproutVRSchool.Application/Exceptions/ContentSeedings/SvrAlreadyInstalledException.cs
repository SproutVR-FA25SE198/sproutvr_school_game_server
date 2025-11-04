namespace SproutVRSchool.Application.Exceptions.ContentSeedings;

public sealed class SvrAlreadyInstalledException : Exception
{
    public SvrAlreadyInstalledException(string message) : base(message)
    {
    }
}
