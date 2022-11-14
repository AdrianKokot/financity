using System.Collections.Immutable;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Financity.Application.Behaviors;

public sealed class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
                                        CancellationToken cancellationToken)
    {
        var errors = GetValidationFailures(request);
        if (errors.Count != 0) throw new ValidationException(errors);

        return await next();
    }

    private IReadOnlyList<ValidationFailure> GetValidationFailures(TRequest request)
    {
        if (!_validators.Any()) return ImmutableList<ValidationFailure>.Empty;

        var context = new ValidationContext<TRequest>(request);

        var errors = _validators.Select(x => x.Validate(context))
                                .SelectMany(x => x.Errors)
                                .Where(f => f is not null)
                                .ToImmutableList();

        return errors;
    }
}