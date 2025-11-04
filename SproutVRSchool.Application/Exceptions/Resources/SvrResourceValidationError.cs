namespace SproutVRSchool.Application.Exceptions.Resources;

public sealed record SvrResourceValidationError
    (string PropertyName, string ErrorMessage);
