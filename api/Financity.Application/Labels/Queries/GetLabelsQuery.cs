using System.Globalization;
using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Common.Queries;
using Financity.Application.Common.Queries.FilteredQuery;
using Financity.Domain.Common;
using Financity.Domain.Entities;

namespace Financity.Application.Labels.Queries;

public sealed class GetLabelsQuery : FilteredEntitiesQuery<LabelListItem>
{
    public GetLabelsQuery(QuerySpecification<LabelListItem> querySpecification) : base(querySpecification)
    {
    }
}

public sealed class GetLabelsQueryHandler : FilteredUserWalletEntitiesQueryHandler<GetLabelsQuery, Label, LabelListItem>
{
    public GetLabelsQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    protected override IQueryable<Label> ExecuteSearch(IQueryable<Label> query, string search)
    {
        search = search.ToLower(CultureInfo.InvariantCulture);
        return query.Where(x => x.Name.ToLower().Contains(search));
    }
}

public sealed record LabelListItem
    (Guid Id, string Name, Guid WalletId, Appearance Appearance) : IMapFrom<Label>;