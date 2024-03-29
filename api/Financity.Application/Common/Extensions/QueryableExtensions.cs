﻿using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Financity.Application.Common.Queries;

namespace Financity.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyQuerySpecification<T, TQ>(this IQueryable<T> query,
                                                               QuerySpecification<TQ> specification)
        where T : class
    {
        return query
               .Filter(specification.Filters)
               .Sort(specification.Sort)
               .Paginate(specification.Pagination);
    }

    private static IQueryable<T> Paginate<T>(this IQueryable<T> query, PaginationSpecification pagination)
        where T : class
    {
        return query
               .Skip(pagination.Skip)
               .Take(pagination.Take);
    }

    private static IQueryable<T> Sort<T>(this IQueryable<T> query, SortSpecification sort) where T : class
    {
        return sort.Direction == ListSortDirection.Ascending
            ? query.OrderBy(PropertySelector<T>(sort.OrderBy))
            : query.OrderByDescending(PropertySelector<T>(sort.OrderBy));
    }

    private static IQueryable<T> Filter<T>(this IQueryable<T> query, IReadOnlyCollection<Filter> filters)
        where T : class
    {
        if (!filters.Any()) return query;

        var entity = typeof(T);
        var entityProperties = entity.GetProperties()
                                     .ToDictionary(x => x.Name, x => x);

        var parameter = Expression.Parameter(entity);

        var expression = filters.Where(x => entityProperties.ContainsKey(x.Key))
                                .Select(x =>
                                    GenerateFilterExpression(
                                        x,
                                        Expression.Property(parameter, x.Key),
                                        GetProperType(entityProperties[x.Key], x.Value)
                                    )
                                )
                                .Aggregate(Expression.And);

        var lambda = Expression.Lambda<Func<T, bool>>(expression, parameter);

        return query.Where(lambda);
    }

    private static Expression GetExpressionCallForOperator(string op, Expression left, Expression right)
    {
        if (op != FilterOperators.Contain)
            throw new InvalidOperationException(
                $"Filter operator '{op}' is not a valid operator.");

        var toLowerMethodInfo = typeof(string).GetMethod(nameof(string.ToLower), Array.Empty<Type>());
        var containsMethodInfo = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });

        return Expression.Call(
            Expression.Call(left, toLowerMethodInfo!),
            containsMethodInfo!,
            Expression.Call(right, toLowerMethodInfo!)
        );
    }

    private static Expression GenerateFilterExpression(Filter filter, Expression left, Expression right)
    {
        return filter.Operator switch
        {
            FilterOperators.Equal => Expression.Equal(left, right),
            FilterOperators.NotEqual => Expression.NotEqual(left, right),
            FilterOperators.LessOrEqual => Expression.LessThanOrEqual(left, right),
            FilterOperators.GreaterOrEqual => Expression.GreaterThanOrEqual(left, right),
            _ => GetExpressionCallForOperator(filter.Operator, left, right)
        };
    }

    private static ConstantExpression GetProperType(PropertyInfo info, string value)
    {
        if (info.PropertyType == typeof(DateTime))
            return Expression.Constant(DateTime.Parse(value, DateTimeFormatInfo.InvariantInfo,
                DateTimeStyles.AssumeUniversal).ToUniversalTime());

        if (info.PropertyType == typeof(DateOnly))
        {
            var parsedDate = DateOnly.FromDateTime(DateTime.Parse(value).ToUniversalTime());

            if (DateOnly.TryParse(value, out var successfullyParsed)) parsedDate = successfullyParsed;

            return Expression.Constant(parsedDate);
        }

        if (info.PropertyType == typeof(Guid))
            return Expression.Constant(Guid.Parse(value));

        if (info.PropertyType.IsEnum)
            return Expression.Constant(Enum.Parse(info.PropertyType, value));

        return Expression.Constant(value);
    }

    private static Expression<Func<T, object>> PropertySelector<T>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, propertyName);
        var propAsObject = Expression.Convert(property, typeof(object));
        return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
    }
}