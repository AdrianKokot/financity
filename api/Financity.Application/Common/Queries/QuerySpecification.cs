namespace Financity.Application.Common.Queries;

public sealed class QuerySpecification
{
    public PaginationSpecification PaginationSpecification { get; set; } = new();
    public SortSpecification SortSpecification { get; set; } = new();
    private IEnumerable<Filter> Filters { get; set; } = new List<Filter>();
}