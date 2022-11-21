using System.ComponentModel;
using Financity.Application.Common.Queries;
using Financity.Domain.Common;
using System.Linq.Expressions;

namespace Financity.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyQuerySpecification<T, TQ>(this IQueryable<T> query,
                                                               QuerySpecification<TQ> specification)
        where T : IEntity
    {
        return query
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

    private static Expression<Func<T, object>> PropertySelector<T>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, propertyName);
        var propAsObject = Expression.Convert(property, typeof(object));
        return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
    }
}