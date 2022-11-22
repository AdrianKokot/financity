namespace Financity.Application.Common.Queries;

public sealed class Filter
{
    public string Key { get; set; } = string.Empty;
    public string Operator { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}