namespace Financity.Application.Common.Queries;

public sealed class PaginationSpecification
{
    private int _take = 20;

    public int Take
    {
        get => _take;
        set => _take = Math.Clamp(value, 1, 250);
    }

    public int Skip { get; set; } = 0;
}