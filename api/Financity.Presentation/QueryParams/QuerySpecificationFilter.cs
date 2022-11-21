using System.Net.Mime;
using Financity.Application.Common.Queries;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Financity.Presentation.QueryParams;

public sealed class QuerySpecificationFilter : IOperationFilter
{
    private static readonly IDictionary<Type, OpenApiSchema> ApiSchemas = new Dictionary<Type, OpenApiSchema>()
    {
        {typeof(int), new OpenApiSchema() {Type = "integer", Format = nameof(Int32).ToLower()}},
        {typeof(string), new OpenApiSchema() {Type = nameof(String).ToLower()}}
    };

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasQuerySpecificationBinding = context.ApiDescription.ActionDescriptor.Parameters
                                                  .Any(x => x.ParameterType.IsGenericType &&
                                                            x.ParameterType
                                                             .GetGenericTypeDefinition() == typeof(QuerySpecification<>)
                                                  );

        if (!hasQuerySpecificationBinding)
        {
            return;
        }

        var querySpecificationProperties = typeof(QuerySpecification<>).GetProperties().Select(x => x.Name).ToHashSet();

        operation.Parameters = operation.Parameters
                                        .Where(x => !querySpecificationProperties.Contains(x.Name.Split(".").First()))
                                        .ToList();
        
        (Type Type, string Name)[] integerBindings =
        {
            (typeof(int), QueryParamKeys.PageQueryParamKey),
            (typeof(int), QueryParamKeys.PageSizeQueryParamKey),
            (typeof(string), QueryParamKeys.OrderByQueryParamKey),
            (typeof(string), QueryParamKeys.OrderByDirectionQueryParamKey),
        };

        foreach (var binding in integerBindings)
        {
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = binding.Name,
                In = ParameterLocation.Query,
                Required = false,
                Schema = ApiSchemas[binding.Type]
            });
        }
    }
}