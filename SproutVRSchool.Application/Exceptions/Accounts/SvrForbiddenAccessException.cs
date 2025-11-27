namespace SproutVRSchool.Application.Exceptions.Accounts;

public sealed class SvrForbiddenAccessException : Exception
{
    public SvrForbiddenAccessException(string? message) : base(message)
    {
    }
}
