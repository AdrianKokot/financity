using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Queries.DetailsQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Recipients.Queries;

public sealed record GetRecipientQuery(Guid EntityId) : IEntityQuery<RecipientDetails>;

public sealed class GetRecipientQueryHandler : EntityQueryHandler<GetRecipientQuery, Recipient, RecipientDetails>
{
    public GetRecipientQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }
}

public sealed record RecipientDetails(Guid Id, string Name, Guid WalletId, string WalletName);