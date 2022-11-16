using AutoMapper;
using AutoMapper.QueryableExtensions;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Extensions;
using Financity.Application.Common.Queries;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Transactions.Queries;

public sealed class SearchTransactionsQuery : IQuery<IEnumerable<TransactionListItem>>
{
    public QuerySpecification QuerySpecification { get; set; } = new();
    public Guid UserId { get; set; }
    public Guid? WalletId { get; set; } = null;
    public string SearchTerm { get; set; } = string.Empty;
}

public sealed class
    SearchTransactionsQueryHandler : IQueryHandler<SearchTransactionsQuery, IEnumerable<TransactionListItem>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _userService;

    public SearchTransactionsQueryHandler(IApplicationDbContext dbContext, ICurrentUserService userService,
                                          IMapper mapper)
    {
        _dbContext = dbContext;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TransactionListItem>> Handle(SearchTransactionsQuery request, CancellationToken ct)
    {
        request.UserId = _userService.UserId;

        return await _dbContext.SearchUserTransactions(request.UserId, request.SearchTerm, request.WalletId)
                               .AsNoTracking()
                               .Paginate(request.QuerySpecification.PaginationSpecification)
                               .ProjectTo<TransactionListItem>(_mapper.ConfigurationProvider)
                               .ToListAsync(ct);
    }
}