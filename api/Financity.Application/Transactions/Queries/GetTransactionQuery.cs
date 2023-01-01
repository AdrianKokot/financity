using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Categories.Queries;
using Financity.Application.Common.Queries.DetailsQuery;
using Financity.Application.Labels.Queries;
using Financity.Application.Recipients.Queries;
using Financity.Application.Wallets.Queries;
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

public sealed record TransactionDetails(Guid Id, Guid WalletId, WalletDetails Wallet, decimal Amount, string Note,
                                        Guid? RecipientId, RecipientDetails? Recipient, string TransactionType,
                                        DateTime TransactionDate, Guid? CategoryId, CategoryDetails? Category,
                                        ICollection<LabelDetails> Labels, string CurrencyId,
                                        decimal ExchangeRate) : IMapFrom<Transaction>;