using System.Globalization;
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
        search = search.Trim().ToLower(CultureInfo.InvariantCulture);
        var numberSearch = search.Replace(',', '.').Replace("-", "");

        return query.Where(x =>
            x.Note.ToLower().Contains(search)
            || (x.Category == null ? string.Empty : x.Category.Name).ToLower().Contains(search)
            || x.Labels.Any(y => y.Name.ToLower().Contains(search))
            || x.Amount.ToString().Contains(numberSearch)
            || (x.Amount * x.ExchangeRate).ToString().Contains(numberSearch)
            || x.Currency.Name.ToLower().Contains(search)
            || x.CurrencyId.ToLower().Contains(search)
            || (x.Recipient == null ? string.Empty : x.Recipient.Name.ToLower()).Contains(search)
        );
    }
}

public sealed class TransactionListItem : IMapFrom<Transaction>
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Note { get; set; }
    public Guid? RecipientId { get; set; }
    public string? RecipientName { get; set; }
    public Guid WalletId { get; set; }
    public string WalletName { get; set; }
    public string TransactionType { get; set; }
    public DateTime TransactionDate { get; set; }
    public Guid? CategoryId { get; set; }
    public CategoryListItem? Category { get; set; }
    public ICollection<LabelListItem> Labels { get; set; }
    public string CurrencyName { get; set; }
    public string CurrencyId { get; set; }
    public decimal ExchangeRate { get; set; }

    public static void CreateMap(Profile profile)
    {
        profile.CreateMap<Transaction, TransactionListItem>()
               .ForMember(x => x.TransactionDate,
                   x => x.MapFrom(y => y.TransactionDate.ToDateTime(TimeOnly.MinValue).ToUniversalTime())
               );
    }
}