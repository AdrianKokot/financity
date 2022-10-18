using Financity.Domain.Shared;

namespace Financity.Domain.Entities;

public sealed class Currency : Entity
{
    public Currency() : base(Guid.NewGuid())
    {
    }

    public string Code { get; init; }
    public string Name { get; init; }
}