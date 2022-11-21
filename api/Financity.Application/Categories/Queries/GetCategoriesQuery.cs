using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Common.Queries;
using Financity.Application.Common.Queries.FilteredQuery;
using Financity.Domain.Common;
using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Linq.Expressions.Expression;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using Parameter = System.Reflection.Metadata.Parameter;

namespace Financity.Application.Categories.Queries;

public sealed class GetCategoriesQuery : FilteredEntitiesQuery<CategoryListItem>
{
    public GetCategoriesQuery(QuerySpecification<CategoryListItem> querySpecification) : base(querySpecification)
    {
    }
}

public sealed class
    GetCategoriesQueryHandler : FilteredUserWalletEntitiesQueryHandler<GetCategoriesQuery, Category, CategoryListItem>
{
    public GetCategoriesQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

public sealed record CategoryListItem(Guid Id, string Name, Guid WalletId, Appearance Appearance) : IMapFrom<Category>;