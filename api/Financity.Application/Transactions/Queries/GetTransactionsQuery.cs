using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Categories.Queries;
using Financity.Application.Common.Queries;
using Financity.Application.Common.Queries.FilteredQuery;
using Financity.Application.Labels.Queries;
using Financity.Domain.Entities;

namespace Financity.Application.Transactions.Queries;

public sealed class GetTransactionsQuery : FilteredEntitiesQuery<TransactionListItem>
{
    public GetTransactionsQuery(QuerySpecification<TransactionListItem> querySpecification) : base(querySpecification)
    {
    }
}

public sealed class
    GetTransactionsQueryHandler : FilteredUserWalletEntitiesQueryHandler<GetTransactionsQuery, Transaction,
        TransactionListItem>
{
    public GetTransactionsQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

public sealed record TransactionListItem(Guid Id, decimal Amount, string Note, Guid? RecipientId, string? RecipientName,
                                         Guid WalletId, string WalletName, string TransactionType,
                                         DateTime TransactionDate,
                                         Guid? CategoryId, CategoryListItem? Category,
                                         ICollection<LabelListItem> Labels, string CurrencyName,
                                         string CurrencyId, decimal ExchangeRate) : IMapFrom<Transaction>;