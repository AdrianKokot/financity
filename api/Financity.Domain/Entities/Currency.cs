using Financity.Domain.Shared;

namespace Financity.Domain.Entities;

public sealed class Currency : Entity
{
    public string Code { get; }
    public string Name { get; }

    public Currency() : base(Guid.NewGuid())
    {
    }
}