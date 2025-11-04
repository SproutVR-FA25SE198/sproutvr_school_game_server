namespace SproutVRSchool.Application.Exceptions.Resources;

// Thrown when a specific entity (like a session or user) cannot be found.
public sealed class SvrResourceNotFoundException : Exception
{
    public SvrResourceNotFoundException(string? message) : base(message)
    {
    }
}
