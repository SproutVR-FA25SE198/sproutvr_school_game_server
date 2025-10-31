namespace SproutVRSchool.Application.Exceptions;

public sealed record SvrValidationError
    (string PropertyName, string ErrorMessage);
