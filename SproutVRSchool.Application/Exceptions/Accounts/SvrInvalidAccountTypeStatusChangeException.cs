namespace SproutVRSchool.Application.Exceptions.Accounts;

public sealed class SvrInvalidAccountTypeStatusChangeException : Exception
{
    public SvrInvalidAccountTypeStatusChangeException(string? message) : base(message)
    {
    }
}
