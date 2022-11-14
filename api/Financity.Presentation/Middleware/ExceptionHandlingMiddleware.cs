using System.Collections.Immutable;
using Financity.Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Financity.Presentation.Middleware;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            await HandleExceptionAsync(context, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);
        var response = GetErrorResponseFromException(exception);

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(response);
    }

    private static object GetErrorResponseFromException(Exception exception)
    {
        var statusCode = GetStatusCode(exception);
        return statusCode switch
        {
            StatusCodes.Status400BadRequest or StatusCodes.Status422UnprocessableEntity =>
                new ValidationProblemDetails(GetErrors(exception))
                {
                    Status = statusCode,
                    Title = GetTitle(exception),
                    Type = GetExceptionType(exception)
                },
            _ =>
                new
                {
                    Title = GetTitle(exception),
                    Type = GetExceptionType(exception),
                    Status = statusCode,
                    Message = exception.ToString()
                }
        };
    }

    private static int GetStatusCode(Exception exception)
    {
        return exception switch
        {
            EntityNotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            NotImplementedException => StatusCodes.Status501NotImplemented,
            AccessDeniedException => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private static string GetTitle(Exception exception)
    {
        return exception switch
        {
            ValidationException => "One or more validation errors occurred.",
            NotImplementedException => "Not Implemented.",
            AccessDeniedException => "Forbidden.",
            _ => "Internal server error."
        };
    }

    private static string GetExceptionType(Exception exception)
    {
        return exception switch
        {
            ValidationException => "https://httpwg.org/specs/rfc9110.html#status.422",
            NotImplementedException => "https://www.rfc-editor.org/rfc/rfc7231#section-6.6.2",
            AccessDeniedException => "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.3",
            _ => "https://www.rfc-editor.org/rfc/rfc7231#section-6.6.1"
        };
    }

    private static IDictionary<string, string[]> GetErrors(Exception exception)
    {
        IDictionary<string, string[]> errors = ImmutableDictionary<string, string[]>.Empty;

        if (exception is ValidationException validationException)
            errors = validationException.Errors.GroupBy(
                                            x => x.PropertyName,
                                            x => x.ErrorMessage,
                                            (propertyName, errorMessages) => new
                                            {
                                                Key = propertyName,
                                                Values = errorMessages.Distinct().ToArray()
                                            })
                                        .ToDictionary(x => x.Key, x => x.Values);

        return errors;
    }
}