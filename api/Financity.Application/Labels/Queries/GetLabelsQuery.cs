using Financity.Application.Abstractions.Data;
using Financity.Application.Common.FilteredQuery;
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

public sealed record LabelListItem(Guid Id, string Name, Guid WalletId, string? Color, string? IconName);