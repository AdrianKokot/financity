using System.Collections.Immutable;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using Financity.Application.Common.Queries;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Financity.Presentation.QueryParams;

public static class QueryKeys
{
    public const string PageSizeQueryParamKey = "pageSize";
    public const string PageQueryParamKey = "page";
    public const string OrderByQueryParamKey = "orderBy";
    public const string OrderByDirectionQueryParamKey = "direction";
    public const string SearchQueryParamKey = "search";

    public static readonly HashSet<Type> AllowedFilterKeyTypes =
        new() { typeof(Guid), typeof(string), typeof(DateTime), typeof(DateOnly) };
}

public static class QueryParamFilters
{
    public static readonly IDictionary<Type, IEnumerable<string>> TypeOperators =
        new Dictionary<Type, IEnumerable<string>>
        {
            {typeof(Guid), new[] {FilterOperators.Equal, FilterOperators.NotEqual}},
            {typeof(string), new[] {FilterOperators.Equal, FilterOperators.NotEqual, FilterOperators.Contain}},
            {
                typeof(DateTime),
                new[]
                {
                    FilterOperators.Equal, FilterOperators.NotEqual, FilterOperators.GreaterOrEqual,
                    FilterOperators.LessOrEqual
                }
            },
            {
                typeof(DateOnly),
                new[]
                {
                    FilterOperators.Equal, FilterOperators.NotEqual, FilterOperators.GreaterOrEqual,
                    FilterOperators.LessOrEqual
                }
            }
        };
}

public sealed class QuerySpecificationBinder<T> : IModelBinder
{
    private readonly IDictionary<string, PropertyInfo> _entityProperties;
    private readonly IObjectModelValidator _validator;

    public QuerySpecificationBinder(IObjectModelValidator validator)
    {
        _validator = validator;
        _entityProperties = typeof(T)
                            .GetProperties()
                            .Where(x => QueryKeys.AllowedFilterKeyTypes.Contains(x.PropertyType))
                            .ToDictionary(x => x.Name.ToLower(), x => x);
    }

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var model = new QuerySpecification<T>
        {
            Pagination = ParsePagination(bindingContext.ValueProvider),
            Sort = ParseSort(bindingContext.ValueProvider),
            Filters = ParseFilters(bindingContext),
            Search = ParseSearch(bindingContext.ValueProvider)
        };

        bindingContext.Result = ModelBindingResult.Success(model);
        _validator.Validate(bindingContext.ActionContext, bindingContext.ValidationState, string.Empty, model);

        await Task.CompletedTask;
    }

    private static string ParseSearch(IValueProvider valueProvider)
    {
        return valueProvider.GetValue(QueryKeys.SearchQueryParamKey).FirstValue ?? string.Empty;
    }

    private static PaginationSpecification ParsePagination(IValueProvider valueProvider)
    {
        var specification = new PaginationSpecification();

        var pageSizeString = valueProvider.GetValue(QueryKeys.PageSizeQueryParamKey).FirstValue;

        if (pageSizeString is not null && int.TryParse(pageSizeString, null, out var pageSize))
            specification.Take = pageSize;

        var pageString = valueProvider.GetValue(QueryKeys.PageQueryParamKey).FirstValue;
        if (pageString is not null && int.TryParse(pageString, null, out var page))
            specification.Skip = specification.Take * Math.Clamp(page - 1, 0, int.MaxValue);

        return specification;
    }

    private SortSpecification ParseSort(IValueProvider valueProvider)
    {
        var specification = new SortSpecification();

        var orderByString = (valueProvider.GetValue(QueryKeys.OrderByQueryParamKey).FirstValue ?? string.Empty)
            .ToLower(CultureInfo.InvariantCulture);

        specification.OrderBy = _entityProperties.TryGetValue(orderByString, out var info) ? info.Name : "Id";

        var directionString = valueProvider.GetValue(QueryKeys.OrderByDirectionQueryParamKey).FirstValue ??
                              string.Empty;

        specification.Direction = directionString.ToLower().StartsWith("desc")
            ? ListSortDirection.Descending
            : ListSortDirection.Ascending;

        return specification;
    }

    private IReadOnlyCollection<Filter> ParseFilters(ModelBindingContext bindingContext)
    {
        return bindingContext.HttpContext.Request.Query.Keys
                             .Where(x => x.Contains('_'))
                             .Select(x =>
                             {
                                 var propertyNameString = x[..x.IndexOf('_')].ToLower();
                                 var operatorString = x[(propertyNameString.Length + 1)..].ToLower();

                                 if (!_entityProperties.TryGetValue(propertyNameString, out var property))
                                     return null;

                                 if (!QueryParamFilters.TypeOperators[property.PropertyType]
                                                       .Contains(operatorString))
                                     return null;

                                 return new Filter
                                 {
                                     Key = property.Name,
                                     Operator = operatorString,
                                     Value = bindingContext.ValueProvider.GetValue(x).FirstValue!
                                 };
                             })
                             .Where(x => x is not null)
                             .ToImmutableList()!;
    }
}

public sealed class QuerySpecificationBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        if (!(context.Metadata.ModelType.IsGenericType &&
              context.Metadata.ModelType.GetGenericTypeDefinition() == typeof(QuerySpecification<>)))
            return null;

        var generic = context.Metadata.ModelType.GetGenericArguments().FirstOrDefault();

        if (generic is null) return null;

        var binderType = typeof(QuerySpecificationBinder<>).MakeGenericType(generic);

        return new BinderTypeModelBinder(binderType);
    }
}