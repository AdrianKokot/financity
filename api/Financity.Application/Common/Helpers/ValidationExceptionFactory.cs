using FluentValidation;
using FluentValidation.Results;

namespace Financity.Application.Common.Helpers;

public static class ValidationExceptionFactory
{
    public static ValidationException For(string propertyName)
    {
        return For(propertyName, $"{propertyName} is invalid.");
    }

    public static ValidationException For(string propertyName, string message)
    {
        return new ValidationException(new[] { new ValidationFailure(propertyName, message) });
    }
}