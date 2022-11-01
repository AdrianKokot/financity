using Financity.Application.Abstractions.Data;
using Financity.Application.Common.FilteredQuery;
using Financity.Application.Common.Mappings;
using Financity.Domain.Entities;

namespace Financity.Application.Labels.Queries;

public sealed class GetLabelsQuery : FilteredEntitiesQuery<LabelListItem>
{
    public GetLabelsQuery(QuerySpecification querySpecification) : base(querySpecification)
    {
    }
}

public sealed class GetLabelsQueryHandler : FilteredEntitiesQueryHandler<GetLabelsQuery, Label, LabelListItem>
{
    public GetLabelsQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }
}

public sealed class LabelListItem : IMapFrom<Label>
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public Guid WalletId { get; init; }
}