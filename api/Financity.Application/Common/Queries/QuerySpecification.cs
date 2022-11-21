namespace Financity.Application.Common.Queries;

public sealed class QuerySpecification<T>
{
    public PaginationSpecification Pagination { get; set; } = new();
    public SortSpecification Sort { get; set; } = new();
    private IEnumerable<Filter> Filters { get; set; } = new List<Filter>();
}