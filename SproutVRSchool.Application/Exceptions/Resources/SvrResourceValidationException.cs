namespace SproutVRSchool.Application.Exceptions.Resources;

// Throws when one or more validation errors occur from FluentValidator
public sealed class SvrResourceValidationException : Exception
{
    public IEnumerable<SvrResourceValidationError> Errors { get; }

    public SvrResourceValidationException(IEnumerable<SvrResourceValidationError> validationErrors)
    {
        Errors = validationErrors;
    }
}
