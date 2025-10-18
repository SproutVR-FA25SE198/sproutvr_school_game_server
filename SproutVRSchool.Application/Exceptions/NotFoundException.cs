namespace SproutVRSchool.Application.Exceptions;

// Thrown when a specific entity (like a session or user) cannot be found.
public sealed class NotFoundException : Exception
{
    public NotFoundException(string? message) : base(message)
    {
    }
}
