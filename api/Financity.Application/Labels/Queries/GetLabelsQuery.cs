using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Mappings;
using Financity.Application.Common.Queries;
using Financity.Application.Common.Queries.FilteredQuery;
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
    public GetLabelsQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

public sealed record LabelListItem
    (Guid Id, string Name, Guid WalletId, string? Color, string? IconName) : IMapFrom<Label>;