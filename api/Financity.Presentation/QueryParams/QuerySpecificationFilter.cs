using System.Net.Mime;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Financity.Presentation.QueryParams;

public sealed class QuerySpecificationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var p = context.ApiDescription.ActionDescriptor.Parameters
                       .Where(x => x.BindingInfo.BinderType == typeof(QuerySpecificationBinder))
                       .Select(x => x.Name)
                       .ToHashSet();

        if (!p.Any())
        {
            return;
        }

        operation.Parameters.Add(new OpenApiParameter()
        {
            Name = "page",
            In = ParameterLocation.Query,
            Required = false,
            Schema = new OpenApiSchema()
            {
                Type = "integer",
                Format = nameof(Int32).ToLower()
            },
        });
        
    }
}