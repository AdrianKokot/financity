using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Mappings;
using Financity.Application.Common.Queries.DetailsQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Budgets.Queries;

public sealed record GetBudgetQuery(Guid EntityId) : IEntityQuery<BudgetDetails>;

public sealed class GetBudgetQueryHandler : EntityQueryHandler<GetBudgetQuery, Budget, BudgetDetails>
{
    public GetBudgetQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

public sealed record BudgetDetails(Guid Id, string Name, decimal Amount, Guid UserId) : IMapFrom<Budget>;