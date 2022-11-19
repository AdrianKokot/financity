using AutoMapper;
using AutoMapper.QueryableExtensions;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Categories.Queries;
using Financity.Application.Common.Exceptions;
using Financity.Application.Common.Queries.DetailsQuery;
using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Budgets.Queries;

public sealed record GetBudgetQuery(Guid EntityId) : IEntityQuery<BudgetDetails>;

public sealed class GetBudgetQueryHandler : EntityQueryHandler<GetBudgetQuery, Budget, BudgetDetails>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetBudgetQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public override async Task<BudgetDetails> Handle(GetBudgetQuery request, CancellationToken cancellationToken)
    {
        var currentDate = AppDateTime.Now.ToUniversalTime();
        var entity = await _dbContext.GetDbSet<Budget>()
                                     .AsNoTracking()
                                     .Where(x => x.Id == request.EntityId && x.UserId == _dbContext.UserService.UserId)
                                     .Include(x => x.TrackedCategories)
                                     .ThenInclude(x => x.Transactions.Where(y =>
                                         y.TransactionDate.Year == currentDate.Year &&
                                         y.TransactionDate.Month == currentDate.Month))
                                     .ProjectTo<BudgetDetails>(_mapper.ConfigurationProvider)
                                     .FirstOrDefaultAsync(cancellationToken);

        if (entity is null) throw new EntityNotFoundException(nameof(Budget), request.EntityId);

        return entity;
    }
}

public sealed record BudgetDetails(Guid Id, string Name, decimal Amount, Guid UserId,
                                   IEnumerable<CategoryListItem> TrackedCategories,
                                   decimal CurrentPeriodExpenses) : IMapFrom<Budget>;