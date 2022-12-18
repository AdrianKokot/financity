using System.Globalization;
using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Common.Queries;
using Financity.Application.Common.Queries.FilteredQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Recipients.Queries;

public sealed class GetRecipientsQuery : FilteredEntitiesQuery<RecipientListItem>
{
    public GetRecipientsQuery(QuerySpecification<RecipientListItem> querySpecification) : base(querySpecification)
    {
    }
}

public sealed class
    GetRecipientsQueryHandler : FilteredUserWalletEntitiesQueryHandler<GetRecipientsQuery, Recipient, RecipientListItem>
{
    public GetRecipientsQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    protected override IQueryable<Recipient> ExecuteSearch(IQueryable<Recipient> query, string search)
    {
        search = search.ToLower(CultureInfo.InvariantCulture);
        return query.Where(x => x.Name.ToLower().Contains(search));
    }
}

public sealed record RecipientListItem(Guid Id, string Name, Guid WalletId) : IMapFrom<Recipient>;