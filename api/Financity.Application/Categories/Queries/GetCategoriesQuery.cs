using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Mappings;
using Financity.Application.Common.Queries;
using Financity.Application.Common.Queries.FilteredQuery;
using Financity.Domain.Common;
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
    public GetCategoriesQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

public sealed record CategoryListItem(Guid Id, string Name, Guid WalletId, Appearance Appearance) : IMapFrom<Category>;