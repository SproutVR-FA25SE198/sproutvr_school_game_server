namespace SproutVRSchool.Application.Exceptions;

public sealed class SvrInstallFailedException : Exception
{
    public SvrInstallFailedException(string message) : base(message)
    {
    }
}
