namespace SproutVRSchool.Application.Exceptions.Accounts;

public sealed class SvrOtpFailureException : Exception
{
    public SvrOtpFailureException(string? message) : base(message)
    {
    }
}
