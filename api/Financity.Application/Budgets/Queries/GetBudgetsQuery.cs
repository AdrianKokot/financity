using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Common.Queries;
using Financity.Application.Common.Queries.FilteredQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Budgets.Queries;

public sealed class GetBudgetsQuery : FilteredEntitiesQuery<BudgetListItem>
{
    public GetBudgetsQuery(QuerySpecification querySpecification) : base(querySpecification)
    {
    }
}

public sealed class
    GetBudgetsQueryHandler : FilteredEntitiesQueryHandler<GetBudgetsQuery, Budget, BudgetListItem>
{
    public GetBudgetsQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

public sealed record BudgetListItem(Guid Id, string Name, decimal Amount, Guid UserId) : IMapFrom<Budget>;