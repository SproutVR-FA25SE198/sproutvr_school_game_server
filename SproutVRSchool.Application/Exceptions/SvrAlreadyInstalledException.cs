namespace SproutVRSchool.Application.Exceptions;

public sealed class SvrAlreadyInstalledException : Exception
{
    public SvrAlreadyInstalledException(string message) : base(message)
    {
    }
}
