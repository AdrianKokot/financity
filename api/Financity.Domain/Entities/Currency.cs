using Financity.Domain.Common;

namespace Financity.Domain.Entities;

public sealed class Currency : IEntity<string>
{
    public string Name { get; init; } = string.Empty;
    public string Id { get; init; } = string.Empty;
}