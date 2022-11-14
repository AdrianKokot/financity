using System.Collections.Immutable;
using System.Text.Json;
using Financity.Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Financity.Presentation.Middleware;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

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
        var response = new ValidationProblemDetails(GetErrors(exception));
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static int GetStatusCode(Exception exception) => exception switch
    {
        BadHttpRequestException => StatusCodes.Status400BadRequest,
        EntityNotFoundException => StatusCodes.Status404NotFound,
        ValidationException => StatusCodes.Status422UnprocessableEntity,
        _ => StatusCodes.Status500InternalServerError
    };

    private static IDictionary<string, string[]> GetErrors(Exception exception)
    {
        IDictionary<string, string[]> errors = ImmutableDictionary<string, string[]>.Empty;
        
        if (exception is ValidationException validationException)
        {
            errors = validationException.Errors.GroupBy(
                                            x => x.PropertyName,
                                            x => x.ErrorMessage,
                                            (propertyName, errorMessages) => new
                                            {
                                                Key = propertyName,
                                                Values = errorMessages.Distinct().ToArray()
                                            })
                                        .ToDictionary(x => x.Key, x => x.Values);
        }

        return errors;
    }
}