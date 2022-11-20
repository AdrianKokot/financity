using Financity.Application.Common.Queries;
using Financity.Domain.Common;

namespace Financity.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, PaginationSpecification specification)
        where T : Entity
    {
        return query
               .Skip(specification.Skip)
               .Take(specification.Take)
               .OrderBy(x => x.Id);
    }
}