namespace SproutVRSchool.Application.Exceptions.Accounts;

public sealed class SvrPasswordMismatchException : Exception
{
    public SvrPasswordMismatchException(string message)
        : base(message)
    {
    }
}
