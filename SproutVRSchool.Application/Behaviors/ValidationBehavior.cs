using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SproutVRSchool.Application.Exceptions;

namespace SproutVRSchool.Application.Behaviors;

internal sealed class ValidationBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> _validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // If no validator, just proceed
        if (!_validators.Any())
        {
            return await next(cancellationToken);
        }

        // Collecting errors of all validators in under 1 request
        var requestContext = new ValidationContext<TRequest>(request);

        // Parallel execution of async validation
        ValidationResult[] validationResults = await Task.WhenAll(_validators
            .Select(v => v.ValidateAsync(requestContext, cancellationToken)));

        IEnumerable<ValidationError> errors = validationResults
            .Where(r => !r.IsValid && r.Errors.Any())
            .SelectMany(r => r.Errors)
            .Select(err => new ValidationError(err.PropertyName, err.ErrorMessage));

        if (errors.Any())
        {
            throw new SproutVRSchool.Application.Exceptions.ValidationException(errors);
        }

        // If no error, proceed
        return await next(cancellationToken);
    }
}
