using AutoMapper.QueryableExtensions;
using Financity.Application.Common.FilteredQuery;
using Financity.Application.Common.Mappings;
using Financity.Domain.Common;

namespace Financity.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, PaginationSpecification specification)
        where T : Entity
    {
        return query.Take(specification.Take).Skip(specification.Skip).OrderBy(x => x.Id);
    }

    public static IQueryable<TDestination> Project<TSource, TDestination>(this IQueryable<TSource> query)
    {
        return query.ProjectTo<TDestination>(Projection.For<TSource, TDestination>());
    }
}