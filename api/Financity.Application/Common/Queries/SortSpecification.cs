using System.ComponentModel;

namespace Financity.Application.Common.Queries;

public sealed class SortSpecification
{
    public ListSortDirection Direction { get; set; } = ListSortDirection.Descending;
    public string OrderBy { get; set; } = "Id";
}