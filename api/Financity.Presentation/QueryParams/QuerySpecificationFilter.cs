using System.Text.Json;
using Financity.Application.Common.Queries;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Financity.Presentation.QueryParams;

public sealed class QuerySpecificationFilter : IOperationFilter
{
    private static readonly IDictionary<Type, OpenApiSchema> ApiSchemas = new Dictionary<Type, OpenApiSchema>
    {
        {typeof(int), new OpenApiSchema {Type = "integer", Format = "int32"}},
        {typeof(string), new OpenApiSchema {Type = "string"}},
        {typeof(Guid), new OpenApiSchema {Type = "string", Format = "uuid"}},
        {typeof(DateTime), new OpenApiSchema {Type = "string", Format = "date-time"}}
    };

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var parameterDescriptor = context.ApiDescription.ActionDescriptor.Parameters
                                         .FirstOrDefault(x => x.ParameterType.IsGenericType &&
                                                              x.ParameterType
                                                               .GetGenericTypeDefinition() ==
                                                              typeof(QuerySpecification<>)
                                         );

        if (parameterDescriptor is null) return;

        var entityType = parameterDescriptor.ParameterType.GetGenericArguments().FirstOrDefault();

        var querySpecificationProperties = typeof(QuerySpecification<>)
                                           .GetProperties()
                                           .Select(x => x.Name)
                                           .ToHashSet();

        operation.Parameters = operation.Parameters
                                        .Where(x => !querySpecificationProperties.Contains(x.Name.Split(".").First()))
                                        .ToList();

        var bindings = new List<KeyValuePair<Type, string>>
        {
            new(typeof(int), QueryKeys.PageQueryParamKey),
            new(typeof(int), QueryKeys.PageSizeQueryParamKey),
            new(typeof(string), QueryKeys.OrderByQueryParamKey),
            new(typeof(string), QueryKeys.OrderByDirectionQueryParamKey),
            new(typeof(string), QueryKeys.SearchQueryParamKey)
        };

        if (entityType is not null)
        {
            var filters = entityType.GetProperties()
                                    .Where(x => QueryKeys.AllowedFilterKeyTypes.Contains(x.PropertyType))
                                    .Select(x =>
                                        (x.PropertyType, Name: JsonNamingPolicy.CamelCase.ConvertName(x.Name))
                                    )
                                    .SelectMany(p => QueryParamFilters.TypeOperators[p.PropertyType].Select(o =>
                                        new KeyValuePair<Type, string>(p.PropertyType, p.Name + "_" + o)));

            bindings.AddRange(filters);
        }

        foreach (var binding in bindings)
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = binding.Value,
                In = ParameterLocation.Query,
                Required = false,
                Schema = ApiSchemas[binding.Key]
            });
    }
}