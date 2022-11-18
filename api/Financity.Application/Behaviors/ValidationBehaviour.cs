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
        var errors = GetValidationFailuresAsync(request, cancellationToken);
        if (errors.Count != 0) throw new ValidationException(errors);

        return await next();
    }

    private IReadOnlyList<ValidationFailure> GetValidationFailuresAsync(
        TRequest request, CancellationToken ct)
    {
        if (!_validators.Any()) return ImmutableList<ValidationFailure>.Empty;

        var context = new ValidationContext<TRequest>(request);

        return _validators.Select(async x => await x.ValidateAsync(context, ct))
                          .SelectMany(x => x.Result.Errors)
                          .Where(f => f is not null)
                          .ToImmutableList();
    }
}