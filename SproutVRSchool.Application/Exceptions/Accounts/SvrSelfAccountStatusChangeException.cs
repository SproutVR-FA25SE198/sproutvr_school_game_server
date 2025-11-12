namespace SproutVRSchool.Application.Exceptions.Accounts;

public sealed class SvrSelfAccountStatusChangeException : Exception
{
    public SvrSelfAccountStatusChangeException(string? message) : base(message)
    {
    }
}
