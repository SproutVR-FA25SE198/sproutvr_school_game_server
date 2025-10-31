namespace SproutVRSchool.Application.Exceptions;

// Thrown when a specific entity (like a session or user) cannot be found.
public sealed class SvrNotFoundException : Exception
{
    public SvrNotFoundException(string? message) : base(message)
    {
    }
}
