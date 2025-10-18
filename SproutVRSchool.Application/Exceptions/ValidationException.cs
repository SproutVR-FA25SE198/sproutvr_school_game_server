namespace SproutVRSchool.Application.Exceptions;

// Throws when one or more validation errors occur from FluentValidator
public sealed class ValidationException : Exception
{
    public IEnumerable<ValidationError> Errors { get; }

    public ValidationException(IEnumerable<ValidationError> validationErrors)
    {
        Errors = validationErrors;
    }
}
