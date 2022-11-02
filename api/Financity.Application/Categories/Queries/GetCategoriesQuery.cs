using Financity.Application.Abstractions.Data;
using Financity.Application.Common.FilteredQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Categories.Queries;

public sealed class GetCategoriesQuery : FilteredEntitiesQuery<CategoryListItem>
{
    public GetCategoriesQuery(QuerySpecification querySpecification) : base(querySpecification)
    {
    }
}

public sealed class
    GetCategoriesQueryHandler : FilteredEntitiesQueryHandler<GetCategoriesQuery, Category, CategoryListItem>
{
    public GetCategoriesQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }
}

public sealed record CategoryListItem(Guid Id, string Name, Guid WalletId);