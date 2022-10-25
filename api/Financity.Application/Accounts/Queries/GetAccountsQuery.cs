using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;

namespace Financity.Application.Accounts.Queries;

public class GetAccountsQuery : IQuery<IEnumerable<AccountListItem>>
{
}

public sealed class AccountListItem
{
    public Guid Id { get; set; }
}

public sealed class GetAccountsQueryHandler : IQueryHandler<GetAccountsQuery, IEnumerable<AccountListItem>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAccountsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<AccountListItem>> Handle(GetAccountsQuery request,
        CancellationToken cancellationToken)
    {
        return _dbContext.Accounts.Take(20).Select(a => new AccountListItem { Id = a.Id }).ToList();
    }
}