using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Mappings;
using Financity.Application.Common.Queries.DetailsQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Labels.Queries;

public sealed record GetLabelQuery(Guid EntityId) : IEntityQuery<LabelDetails>;

public sealed class GetLabelQueryHandler : EntityQueryHandler<GetLabelQuery, Label, LabelDetails>
{
    public GetLabelQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

public sealed record LabelDetails(Guid Id, string Name, string? Color, string? IconName, Guid WalletId,
                                  string WalletName) : IMapFrom<Label>;