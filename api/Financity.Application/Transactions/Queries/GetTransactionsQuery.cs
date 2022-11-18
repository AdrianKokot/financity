using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Common.Queries;
using Financity.Application.Common.Queries.FilteredQuery;
using Financity.Application.Labels.Queries;
using Financity.Domain.Entities;
using Financity.Domain.Enums;

namespace Financity.Application.Transactions.Queries;

public sealed class GetTransactionsQuery : FilteredEntitiesQuery<TransactionListItem>
{
    public GetTransactionsQuery(QuerySpecification querySpecification) : base(querySpecification)
    {
    }
}

public sealed class
    GetTransactionsQueryHandler : FilteredEntitiesQueryHandler<GetTransactionsQuery, Transaction, TransactionListItem>
{
    public GetTransactionsQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

public sealed record TransactionListItem(Guid Id, decimal Amount, string Note, Guid? RecipientId, string? RecipientName,
                                         Guid WalletId, string WalletName, TransactionType TransactionType,
                                         DateTime TransactionDate,
                                         Guid? CategoryId, string? CategoryName,
                                         ICollection<LabelListItem> Labels, string CurrencyCode, string CurrencyName,
                                         Guid CurrencyId, float ExchangeRate) : IMapFrom<Transaction>;