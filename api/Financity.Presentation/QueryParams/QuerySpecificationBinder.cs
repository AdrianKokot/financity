using System.ComponentModel;
using Financity.Application.Common.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Financity.Presentation.QueryParams;

public static class QueryParamKeys
{
    public const string PageSizeQueryParamKey = "pageSize";
    public const string PageQueryParamKey = "page";
    public const string OrderByQueryParamKey = "orderBy";
    public const string OrderByDirectionQueryParamKey = "direction";

    public static readonly HashSet<Type> AllowedFilterKeyTypes = new() {typeof(Guid), typeof(string), typeof(int)};
}

public sealed class QuerySpecificationBinder<T> : IModelBinder
{
    private readonly IObjectModelValidator _validator;

    public QuerySpecificationBinder(IObjectModelValidator validator)
    {
        _validator = validator;
    }

    private static PaginationSpecification ParsePagination(IValueProvider valueProvider)
    {
        var specification = new PaginationSpecification();

        var pageSizeString = valueProvider.GetValue(QueryParamKeys.PageSizeQueryParamKey).FirstValue;

        if (pageSizeString is not null)
        {
            specification.Take = int.Parse(pageSizeString);
        }

        var pageString = valueProvider.GetValue(QueryParamKeys.PageQueryParamKey).FirstValue;
        if (pageString is not null)
        {
            specification.Skip = specification.Take * Math.Clamp(int.Parse(pageString) - 1, 0, int.MaxValue);
        }

        return specification;
    }

    private SortSpecification ParseSort(IValueProvider valueProvider)
    {
        var specification = new SortSpecification();

        var orderByString = (valueProvider.GetValue(QueryParamKeys.OrderByQueryParamKey).FirstValue ?? "id").ToLower();

        specification.OrderBy = typeof(T)
                                .GetProperties()
                                .FirstOrDefault(x =>
                                    QueryParamKeys.AllowedFilterKeyTypes.Contains(x.PropertyType) &&
                                    x.Name.ToLower() == orderByString)?.Name ?? "Id";

        var directionString = valueProvider.GetValue(QueryParamKeys.OrderByDirectionQueryParamKey).FirstValue ?? string.Empty;
        specification.Direction = directionString.ToLower().StartsWith("desc")
            ? ListSortDirection.Descending
            : ListSortDirection.Ascending;

        return specification;
    }

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var keys = bindingContext.HttpContext.Request.Query.Keys.ToList();

        var model = new QuerySpecification<T>()
        {
            Pagination = ParsePagination(bindingContext.ValueProvider),
            Sort = ParseSort(bindingContext.ValueProvider)
        };

        bindingContext.Result = ModelBindingResult.Success(model);
        _validator.Validate(bindingContext.ActionContext, bindingContext.ValidationState, string.Empty, model);

        Console.WriteLine((model.Pagination.Skip, model.Pagination.Take));
        Console.WriteLine((model.Sort.OrderBy, model.Sort.Direction));

        await Task.CompletedTask;
    }
}

public sealed class QuerySpecificationBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (!(context.Metadata.ModelType.IsGenericType &&
              context.Metadata.ModelType.GetGenericTypeDefinition() == typeof(QuerySpecification<>)))
            return null;

        var generic = context.Metadata.ModelType.GetGenericArguments().FirstOrDefault();

        if (generic is null) return null;

        var binderType = typeof(QuerySpecificationBinder<>).MakeGenericType(new[] {generic});

        return new BinderTypeModelBinder(binderType);
    }
}