using Financity.Domain.Common;

namespace Financity.Domain.Entities;

public sealed class Currency : Entity
{
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
}