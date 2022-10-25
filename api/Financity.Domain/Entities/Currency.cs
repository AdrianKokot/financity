using Financity.Domain.Common;

namespace Financity.Domain.Entities;

public sealed class Currency : Entity
{
    public string Code { get; init; }
    public string Name { get; init; }
}