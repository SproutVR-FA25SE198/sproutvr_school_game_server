namespace SproutVRSchool.Application.Exceptions.Accounts;

public sealed class SvrUnauthorizedAccessException : Exception
{
    public SvrUnauthorizedAccessException(string? message) : base(message)
    {
    }
}
