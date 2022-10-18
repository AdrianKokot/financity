using Financity.Application.Abstractions.Data;
using MediatR;

namespace Financity.Application.Accounts.Queries;

public class GetAccountsQuery : IRequest<IEnumerable<AccountDto>>
{
}

public sealed class AccountDto
{
    public Guid Id { get; set; }
}

public sealed class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, IEnumerable<AccountDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAccountsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<AccountDto>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        return _dbContext.Accounts.Take(20).Select(a => new AccountDto {Id = a.Id}).ToList();
    }
}