using System.ComponentModel;

namespace Financity.Application.Common.Queries;

public sealed class SortSpecification
{
    public string Direction { get; set; } = ListSortDirection.Descending.ToString();
    public string OrderBy { get; set; } = "Id";
}