namespace SproutVRSchool.Application.Exceptions.Accounts;

public sealed class SvrInvalidPasswordFormatException : Exception
{
    public SvrInvalidPasswordFormatException(string message)
        : base(message)
    {
    }
}
