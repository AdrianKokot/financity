using Financity.Application.Common.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Financity.Presentation.QueryParams;

public sealed class QuerySpecificationBinder : IModelBinder
{
    private const string PageSizeQueryParamKey = "pageSize";
    private const string PageQueryParamKey = "page";

    private readonly IObjectModelValidator _validator;

    public QuerySpecificationBinder(IObjectModelValidator validator)
    {
        _validator = validator;
    }

    private static PaginationSpecification ParsePagination(IValueProvider valueProvider)
    {
        var specification = new PaginationSpecification();

        var pageSizeString = valueProvider.GetValue(PageSizeQueryParamKey).FirstValue;

        if (pageSizeString is not null)
        {
            specification.Take = int.Parse(pageSizeString);
        }

        var pageString = valueProvider.GetValue(PageQueryParamKey).FirstValue;
        if (pageString is not null)
        {
            specification.Skip = specification.Take * Math.Clamp(int.Parse(pageString) - 1, 0, int.MaxValue);
        }

        return specification;
    }

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        Console.WriteLine("Binding!");
        var keys = bindingContext.HttpContext.Request.Query.Keys.ToList();

        var model = new QuerySpecification()
        {
            PaginationSpecification = ParsePagination(bindingContext.ValueProvider)
        };

        bindingContext.Result = ModelBindingResult.Success(model);
        _validator.Validate(bindingContext.ActionContext, bindingContext.ValidationState, string.Empty, model);
        
        Console.WriteLine((model.PaginationSpecification.Skip,model.PaginationSpecification.Take));

        await Task.CompletedTask;
    }
}

public sealed class QuerySpecificationAttribute : ModelBinderAttribute
{
    public QuerySpecificationAttribute()
    {
        BinderType = typeof(QuerySpecificationBinder);
    }
}