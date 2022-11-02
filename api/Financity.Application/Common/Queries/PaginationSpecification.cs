namespace Financity.Application.Common.Queries;

public sealed class PaginationSpecification
{
    public PaginationSpecification(int take = 20, int skip = 0)
    {
        Take = take;
        Skip = skip;
    }

    public int Take { get; set; }
    public int Skip { get; set; }
}