using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Queries.DetailsQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Categories.Queries;

public sealed record GetCategoryQuery(Guid EntityId) : IEntityQuery<CategoryDetails>;

public sealed class GetCategoryQueryHandler : EntityQueryHandler<GetCategoryQuery, Category, CategoryDetails>
{
    public GetCategoryQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }
}

public sealed record CategoryDetails(Guid Id, string Name, Guid WalletId, string WalletName);