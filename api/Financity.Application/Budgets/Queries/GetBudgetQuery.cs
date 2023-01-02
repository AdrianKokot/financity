using AutoMapper;
using AutoMapper.QueryableExtensions;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Categories.Queries;
using Financity.Application.Common.Queries.DetailsQuery;
using Financity.Domain.Entities;
using Financity.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Budgets.Queries;

public sealed record GetBudgetQuery(Guid EntityId) : IEntityQuery<BudgetDetails>;

public sealed class GetBudgetQueryHandler : UserEntityQueryHandler<GetBudgetQuery, Budget, BudgetDetails>
{
    public GetBudgetQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public override async Task<BudgetDetails> Handle(GetBudgetQuery query, CancellationToken cancellationToken)
    {
        var currentDate = AppDateTime.Now.ToUniversalTime();

        return await FilterAndMapAsync(
            query,
            q => q.Include(x => x.TrackedCategories)
                  .ThenInclude(x => x.Transactions.Where(y =>
                      y.TransactionDate.Year == currentDate.Year &&
                      y.TransactionDate.Month == currentDate.Month && y.TransactionType == TransactionType.Expense)
                  ),
            cancellationToken);
    }
}

public sealed record BudgetDetails(Guid Id, string Name, decimal Amount, Guid UserId,
                                   IEnumerable<CategoryListItem> TrackedCategories, string CurrencyId,
                                   string CurrencyName, decimal CurrentPeriodExpenses) : IMapFrom<Budget>;