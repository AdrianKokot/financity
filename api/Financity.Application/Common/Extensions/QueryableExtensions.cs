using System.ComponentModel;
using Financity.Application.Common.Queries;
using Financity.Domain.Common;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Financity.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyQuerySpecification<T, TQ>(this IQueryable<T> query,
                                                               QuerySpecification<TQ> specification)
        where T : IEntity
    {
        return query
               .Filter(specification.Filters)
               .Sort(specification.Sort)
               .Paginate(specification.Pagination);
    }

    private static IQueryable<T> Paginate<T>(this IQueryable<T> query, PaginationSpecification pagination)
        where T : IEntity
    {
        return query
               .Skip(pagination.Skip)
               .Take(pagination.Take);
    }

    private static IQueryable<T> Sort<T>(this IQueryable<T> query, SortSpecification sort) where T : IEntity
    {
        return sort.Direction == ListSortDirection.Ascending
            ? query.OrderBy(PropertySelector<T>(sort.OrderBy))
            : query.OrderByDescending(PropertySelector<T>(sort.OrderBy));
    }

    private static IQueryable<T> Filter<T>(this IQueryable<T> query, IReadOnlyCollection<Filter> filters)
        where T : IEntity
    {
        if (!filters.Any()) return query;

        var entity = typeof(T);
        var entityProperties = entity.GetProperties()
                                     .ToDictionary(x => x.Name, x => x);

        var parameter = Expression.Parameter(entity);

        var expression = filters.Where(x => entityProperties.ContainsKey(x.Key))
                                .Select(x =>
                                    {
                                        var property = Expression.Property(parameter, x.Key);
                                        var propertyInfo = entityProperties[x.Key];

                                        return (Expression) Expression.Call(
                                            propertyInfo.PropertyType != typeof(string)
                                                ? Expression.Call(property,
                                                    ToStringMethod(propertyInfo))
                                                : property,
                                            GetMethodForOperator(propertyInfo, x.Operator),
                                            Expression.Constant(x.Value)
                                        );
                                    }
                                )
                                .Aggregate(Expression.And);

        var lambda = Expression.Lambda<Func<T, bool>>(expression, parameter);

        return query.Where(lambda);
    }

    private static MethodInfo ToStringMethod(PropertyInfo property)
    {
        return property.PropertyType.GetMethods().First(x => x.Name == "ToString" && x.GetParameters().Length == 0);
    }

    private static MethodInfo? GetMethodForOperator(PropertyInfo property, string op)
    {
        return op switch
        {
            "eq" => typeof(string).GetMethod("Equals", new[] {typeof(string)}),
            "ct" =>
                typeof(string).GetMethod("Contains", new[] {typeof(string)}),
            _ => throw new ArgumentException($"Operator '{op}' is not supported by {property.PropertyType.Name} type")
        };
    }

    private static Expression<Func<T, object>> PropertySelector<T>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, propertyName);
        var propAsObject = Expression.Convert(property, typeof(object));
        return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
    }
}