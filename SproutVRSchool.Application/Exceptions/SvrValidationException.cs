namespace SproutVRSchool.Application.Exceptions;

// Throws when one or more validation errors occur from FluentValidator
public sealed class SvrValidationException : Exception
{
    public IEnumerable<SvrValidationError> Errors { get; }

    public SvrValidationException(IEnumerable<SvrValidationError> validationErrors)
    {
        Errors = validationErrors;
    }
}
