using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Common.Queries;
using Financity.Application.Common.Queries.FilteredQuery;
using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
    private readonly IApplicationDbContext _dbContext;

    public GetBudgetsQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
        _dbContext = dbContext;
    }

    public override Task<IEnumerable<BudgetListItem>> Handle(GetBudgetsQuery request,
                                                             CancellationToken cancellationToken)
    {
        var currentDate = AppDateTime.Now.ToUniversalTime();
        return FilterAndMapAsync(request, q => q
                                               .Where(x => x.UserId == _dbContext.UserService.UserId)
                                               .Include(x => x.TrackedCategories)
                                               .ThenInclude(x => x.Transactions.Where(y =>
                                                   y.TransactionDate.Year == currentDate.Year &&
                                                   y.TransactionDate.Month == currentDate.Month)), cancellationToken);
    }
}

public sealed record BudgetListItem(Guid Id, string Name, decimal Amount, Guid UserId, decimal CurrentPeriodExpenses) : IMapFrom<Budget>;