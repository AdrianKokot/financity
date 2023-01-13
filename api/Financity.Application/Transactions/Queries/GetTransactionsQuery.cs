using System.ComponentModel;
using System.Globalization;
using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Common.Extensions;
using Financity.Application.Common.Queries;
using Financity.Application.Common.Queries.FilteredQuery;
using Financity.Application.Labels.Queries;
using Financity.Domain.Common;
using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Transactions.Queries;

public sealed class GetTransactionsQuery : FilteredEntitiesQuery<TransactionListItem>
{
    public GetTransactionsQuery(QuerySpecification<TransactionListItem> querySpecification) : base(querySpecification)
    {
    }

    public HashSet<Guid> LabelIds { get; set; } = new();
    public HashSet<Guid> CategoryIds { get; set; } = new();
    public HashSet<Guid> RecipientIds { get; set; } = new();
    public Guid? BudgetId { get; set; }
    public string? WalletCurrencyId { get; set; }
}

public sealed class
    GetTransactionsQueryHandler : FilteredUserWalletEntitiesQueryHandler<GetTransactionsQuery, Transaction,
        TransactionListItem>
{
    public GetTransactionsQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    private IQueryable<Transaction> ApplyAdditionalFilters(IQueryable<Transaction> q, GetTransactionsQuery query)
    {
        if (query.LabelIds.Count > 0) q = q.Where(x => x.Labels.Any(l => query.LabelIds.Contains(l.Id)));

        if (query.CategoryIds.Count > 0) q = q.Where(x => query.CategoryIds.Contains(x.CategoryId ?? Guid.Empty));

        if (query.RecipientIds.Count > 0) q = q.Where(x => query.RecipientIds.Contains(x.RecipientId ?? Guid.Empty));

        if (query.BudgetId is not null)
        {
            var categoryIds = DbContext.GetDbSet<Budget>()
                                       .AsNoTracking()
                                       .Where(x => x.Id == query.BudgetId && x.UserId == DbContext.UserService.UserId)
                                       .SelectMany(b => b.TrackedCategories.Select(c => c.Id))
                                       .ToHashSet();

            q = q.Where(x => categoryIds.Contains(x.CategoryId ?? Guid.Empty));
        }

        if (!string.IsNullOrEmpty(query.WalletCurrencyId))
            q = q.Where(x => x.Wallet.CurrencyId.Equals(query.WalletCurrencyId));

        return q;
    }

    public override Task<IEnumerable<TransactionListItem>> Handle(GetTransactionsQuery query,
                                                                  CancellationToken cancellationToken)
    {
        Func<IQueryable<Transaction>, IQueryable<Transaction>> expression = q => ApplyAdditionalFilters(q, query);

        var expr = expression.Invoke;

        if (!string.IsNullOrEmpty(query.QuerySpecification.Search))
            expr = q => ExecuteSearch(expression.Invoke(q), query.QuerySpecification.Search);

        return AccessAsync(q =>
        {
            var toProject = expr.Invoke(q).ApplyQuerySpecification(query.QuerySpecification);

            if (query.QuerySpecification.Sort.OrderBy == nameof(Transaction.Amount))
                toProject = query.QuerySpecification.Sort.Direction == ListSortDirection.Descending
                    ? toProject.OrderByDescending(x => x.Amount * x.ExchangeRate)
                    : toProject.OrderBy(x => x.Amount * x.ExchangeRate);

            return Project(toProject, query);
        }, cancellationToken);
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

public sealed record TransactionLabel(Guid Id, string Name, LabelAppearance Appearance) : IMapFrom<Label>;

public sealed record TransactionCategory(Guid Id, string Name, Appearance Appearance) : IMapFrom<Category>;

public sealed record TransactionRecipient(Guid Id, string Name) : IMapFrom<Recipient>;

public sealed record TransactionListItem(Guid Id, Guid WalletId, decimal Amount, string Note, Guid? RecipientId,
                                         TransactionRecipient? Recipient, string TransactionType,
                                         DateTime TransactionDate, Guid? CategoryId, TransactionCategory? Category,
                                         ICollection<TransactionLabel> Labels, string CurrencyId,
                                         decimal ExchangeRate) : IMapFrom<Transaction>
{
    public static void CreateMap(Profile profile)
    {
        profile.CreateMap<Transaction, TransactionListItem>()
               .ForMember(x => x.Labels, y => y.MapFrom(x => x.Labels.OrderBy(l => l.Id).Take(5)));
    }
}