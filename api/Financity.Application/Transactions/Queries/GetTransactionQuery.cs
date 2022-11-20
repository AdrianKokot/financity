using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Common.Queries.DetailsQuery;
using Financity.Application.Labels.Queries;
using Financity.Domain.Entities;
using Financity.Domain.Enums;

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
                                        Guid WalletId, string WalletName, TransactionType TransactionType,
                                        DateTime TransactionDate,
                                        Guid? CategoryId, string? CategoryName,
                                        IEnumerable<LabelListItem> Labels, string CurrencyCode, string CurrencyName,
                                        Guid CurrencyId, float ExchangeRate) : IMapFrom<Transaction>;