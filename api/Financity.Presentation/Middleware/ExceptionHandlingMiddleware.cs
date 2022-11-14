﻿using System.Collections.Immutable;
using System.Net.Mime;
using System.Text.Json;
using Financity.Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

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
        object response;

        switch (statusCode)
        {
            case StatusCodes.Status400BadRequest:
            case StatusCodes.Status422UnprocessableEntity:
                response = new ValidationProblemDetails(GetErrors(exception))
                {
                    Status = statusCode,
                    Title = GetTitle(exception),
                    Type = GetExceptionType(exception)
                };
                break;
            default:
                response = new
                {
                    Title = GetTitle(exception),
                    Type = GetExceptionType(exception),
                    Status = statusCode,
                    Message = exception.ToString()
                };
                break;
        }

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(response);
    }

    private static int GetStatusCode(Exception exception)
    {
        return exception switch
        {
            EntityNotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            NotImplementedException => StatusCodes.Status501NotImplemented,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private static string GetTitle(Exception exception)
    {
        return exception switch
        {
            ValidationException => "One or more validation errors occurred.",
            NotImplementedException => "Not Implemented",
            _ => "Internal server error."
        };
    }

    private static string GetExceptionType(Exception exception)
    {
        return exception switch
        {
            ValidationException => "https://httpwg.org/specs/rfc9110.html#status.422",
            NotImplementedException => "https://www.rfc-editor.org/rfc/rfc7231#section-6.6.2",
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