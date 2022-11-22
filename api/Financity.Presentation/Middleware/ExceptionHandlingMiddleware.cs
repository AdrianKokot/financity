using Financity.Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Financity.Presentation.Middleware;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ProblemDetailsFactory _detailsFactory;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger,
                                       IHttpContextAccessor contextAccessor, IWebHostEnvironment environment)
    {
        _detailsFactory = contextAccessor.HttpContext?.RequestServices.GetService<ProblemDetailsFactory>() ??
                          throw new ArgumentNullException(nameof(contextAccessor));
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var response = CreateProblemDetails(httpContext, exception);

        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = GetStatusCode(exception);

        if (httpContext.Response.StatusCode >= StatusCodes.Status500InternalServerError)
            _logger.LogError("Server Exception: {message}\n{stackTrace}", exception.Message, exception.StackTrace);

        await httpContext.Response.WriteAsJsonAsync(response);
    }

    private object CreateProblemDetails(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);

        return statusCode switch
        {
            StatusCodes.Status400BadRequest or StatusCodes.Status422UnprocessableEntity =>
                _detailsFactory.CreateValidationProblemDetails(httpContext, GetModelState(exception), statusCode),
            _ =>
                _detailsFactory.CreateProblemDetails(httpContext, statusCode,
                    detail: _environment.IsDevelopment() ? exception.ToString() : exception.Message)
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
            FormatException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private static ModelStateDictionary GetModelState(Exception exception)
    {
        var model = new ModelStateDictionary();

        if (exception is ValidationException validationException)
            foreach (var error in validationException.Errors)
                model.AddModelError(error.PropertyName, error.ErrorMessage);

        if (exception is FormatException formatException)
            model.AddModelError(string.Empty, formatException.Message);

        return model;
    }
}