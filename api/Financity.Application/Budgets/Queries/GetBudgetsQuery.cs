using System.Globalization;
using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Common.Queries;
using Financity.Application.Common.Queries.FilteredQuery;
using Financity.Domain.Entities;
using Financity.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Budgets.Queries;

public sealed class GetBudgetsQuery : FilteredEntitiesQuery<BudgetListItem>
{
    public GetBudgetsQuery(QuerySpecification<BudgetListItem> querySpecification) : base(querySpecification)
    {
    }
}

public sealed class
    GetBudgetsQueryHandler : FilteredUserEntitiesQueryHandler<GetBudgetsQuery, Budget, BudgetListItem>
{
    public GetBudgetsQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public override Task<IEnumerable<BudgetListItem>> Handle(GetBudgetsQuery query,
                                                             CancellationToken cancellationToken)
    {
        var currentDate = AppDateTime.Now.ToUniversalTime();
        return FilterAndMapAsync(query, q => q
                                             .Include(x => x.TrackedCategories)
                                             .ThenInclude(x => x.Transactions.Where(y =>
                                                 y.TransactionDate.Year == currentDate.Year &&
                                                 y.TransactionDate.Month == currentDate.Month &&
                                                 y.TransactionType == TransactionType.Expense)
                                             ), cancellationToken);
    }

    protected override IQueryable<Budget> ExecuteSearch(IQueryable<Budget> query, string search)
    {
        search = search.ToLower(CultureInfo.InvariantCulture);
        return query.Where(x => x.Name.ToLower().Contains(search));
    }
}

public sealed record BudgetListItem
(Guid Id, string Name, decimal Amount, Guid UserId, decimal CurrentPeriodExpenses,
 string CurrencyId) : IMapFrom<Budget>;