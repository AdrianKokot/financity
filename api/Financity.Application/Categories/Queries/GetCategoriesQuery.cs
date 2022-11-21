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
        // Console.WriteLine(result);

        // dbContext.GetDbSet<Category>().AsNoTracking().Where("", Array.Empty<object>());
    }
    //
    // public override async Task<IEnumerable<CategoryListItem>> Handle(GetCategoriesQuery query,
    //                                                            CancellationToken cancellationToken)
    // {
    //     Type dtoType = typeof(CategoryListItem);
    //     Type entityType = typeof(Category);
    //
    //     var validPropertyTypes = new HashSet<Type>() {typeof(Guid), typeof(string)};
    //     var entityProperties = entityType.GetProperties().Select(x => x.PropertyType.Name + "_" + x.Name).ToHashSet();
    //     var props = dtoType
    //                 .GetProperties()
    //                 .Where(x => validPropertyTypes.Contains(x.PropertyType))
    //                 .Where(x => entityProperties.Contains(x.PropertyType.Name + "_" + x.Name))
    //                 .ToList();
    //
    //     Console.WriteLine(string.Join("\n", props.Select(x => x.Name)));
    //
    //
    //     var filter = new {Key = "Name", Operator = "eq", Value = "custom"};
    //
    //
    //     var prm = Parameter(entityType);
    //     Expression body = Call(
    //         Property(prm, filter.Key),
    //         typeof(string).GetMethod("Contains", new[] {typeof(string)}),
    //         Constant(filter.Value)
    //     );
    //
    //     Console.WriteLine(body.ToString());
    //
    //     Expression<Func<Category, bool>> lambda = Lambda<Func<Category, bool>>(body, prm);
    //
    //     Console.WriteLine(lambda.ToString());
    //     return await DbContext.GetDbSet<Category>().AsNoTracking().Where(lambda)
    //                           .ProjectTo<CategoryListItem>(Mapper.ConfigurationProvider).ToListAsync(cancellationToken);
    //
    // }
}

public sealed record CategoryListItem(Guid Id, string Name, Guid WalletId, Appearance Appearance) : IMapFrom<Category>;