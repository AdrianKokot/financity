using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Common.Queries.DetailsQuery;
using Financity.Domain.Common;
using Financity.Domain.Entities;

namespace Financity.Application.Labels.Queries;

public sealed record GetLabelQuery(Guid EntityId) : IEntityQuery<LabelDetails>;

public sealed class GetLabelQueryHandler : UserWalletEntityQueryHandler<GetLabelQuery, Label, LabelDetails>
{
    public GetLabelQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

public sealed record LabelDetails
    (Guid Id, string Name, Appearance Appearance, Guid WalletId, string WalletName) : IMapFrom<Label>;