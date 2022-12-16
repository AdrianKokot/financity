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

    public HashSet<Guid> LabelIds { get; set; } = new();
    public HashSet<Guid> CategoryIds { get; set; } = new();
    public HashSet<Guid> RecipientIds { get; set; } = new();
}

public sealed class
    GetTransactionsQueryHandler : FilteredUserWalletEntitiesQueryHandler<GetTransactionsQuery, Transaction,
        TransactionListItem>
{
    public GetTransactionsQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    private static IQueryable<Transaction> ApplyIdsFilters(IQueryable<Transaction> q, GetTransactionsQuery query)
    {
        if (query.LabelIds.Count > 0) q = q.Where(x => x.Labels.Any(l => query.LabelIds.Contains(l.Id)));

        if (query.CategoryIds.Count > 0) q = q.Where(x => query.CategoryIds.Contains(x.CategoryId ?? Guid.Empty));

        if (query.RecipientIds.Count > 0) q = q.Where(x => query.RecipientIds.Contains(x.RecipientId ?? Guid.Empty));

        return q;
    }

    public override Task<IEnumerable<TransactionListItem>> Handle(GetTransactionsQuery query,
                                                                  CancellationToken cancellationToken)
    {
        return FilterAndMapAsync(
            query,
            q => ApplyIdsFilters(q, query),
            cancellationToken
        );
    }

    protected override IQueryable<Transaction> ExecuteSearch(IQueryable<Transaction> query, string search)
    {
        search = search.Trim().ToLower();
        var numberSearch = search.Replace(',', '.').Replace("-", "");

        return query.Where(x =>
            x.Note.ToLower().Contains(search)
            || (x.Category == null ? string.Empty : x.Category.Name).ToLower().Contains(search)
            || x.Labels.Any(y => y.Name.ToLower().Contains(search))
            || x.Amount.ToString().Contains(numberSearch)
            || (x.Amount * x.ExchangeRate).ToString().Contains(numberSearch)
            || x.Currency.Name.ToLower().Contains(search)
            || x.CurrencyId.ToLower().Contains(search)
        );
    }
}

public sealed record TransactionListItem(Guid Id, decimal Amount, string Note, Guid? RecipientId, string? RecipientName,
                                         Guid WalletId, string WalletName, string TransactionType,
                                         DateOnly TransactionDate,
                                         Guid? CategoryId, CategoryListItem? Category,
                                         ICollection<LabelListItem> Labels, string CurrencyName,
                                         string CurrencyId, decimal ExchangeRate) : IMapFrom<Transaction>;