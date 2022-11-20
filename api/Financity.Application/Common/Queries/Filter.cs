namespace Financity.Application.Common.Queries;

public sealed class Filter
{
    public string Key { get; set; }
    public string Operator { get; set; }
    public string Value { get; set; }
}