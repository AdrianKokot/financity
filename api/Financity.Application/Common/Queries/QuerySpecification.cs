namespace Financity.Application.Common.Queries;

public sealed class QuerySpecification
{
    public PaginationSpecification PaginationSpecification { get; set; } = new();
}