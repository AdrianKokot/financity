using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Queries;
using Financity.Application.Common.Queries.FilteredQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Recipients.Queries;

public sealed class GetRecipientsQuery : FilteredEntitiesQuery<RecipientListItem>
{
    public GetRecipientsQuery(QuerySpecification querySpecification) : base(querySpecification)
    {
    }
}

public sealed class
    GetRecipientsQueryHandler : FilteredEntitiesQueryHandler<GetRecipientsQuery, Recipient, RecipientListItem>
{
    public GetRecipientsQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }
}

public sealed record RecipientListItem(Guid Id, string Name, Guid WalletId);