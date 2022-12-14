using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Categories.Queries;
using Financity.Application.Common.Queries.DetailsQuery;
using Financity.Application.Labels.Queries;
using Financity.Domain.Entities;

namespace Financity.Application.Transactions.Queries;

public sealed record GetTransactionQuery(Guid EntityId) : IEntityQuery<TransactionDetails>;

public sealed class
    GetTransactionQueryHandler : UserWalletEntityQueryHandler<GetTransactionQuery, Transaction, TransactionDetails>
{
    public GetTransactionQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

public sealed record TransactionDetails(Guid Id, decimal Amount, string Note, Guid? RecipientId, string? RecipientName,
                                        Guid WalletId, string WalletName, string TransactionType,
                                        DateOnly TransactionDate,
                                        Guid? CategoryId, CategoryListItem? Category,
                                        IEnumerable<LabelListItem> Labels, string CurrencyName,
                                        string CurrencyId, decimal ExchangeRate) : IMapFrom<Transaction>;