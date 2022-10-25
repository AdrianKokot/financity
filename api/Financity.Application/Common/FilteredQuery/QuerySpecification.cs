namespace Financity.Application.Common.FilteredQuery;

public sealed class QuerySpecification
{
    public PaginationSpecification PaginationSpecification { get; set; } = new();
}