﻿using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Common.Queries.DetailsQuery;
using Financity.Domain.Common;
using Financity.Domain.Entities;

namespace Financity.Application.Categories.Queries;

public sealed record GetCategoryQuery(Guid EntityId) : IEntityQuery<CategoryDetails>;

public sealed class GetCategoryQueryHandler : UserWalletEntityQueryHandler<GetCategoryQuery, Category, CategoryDetails>
{
    public GetCategoryQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

public sealed record CategoryDetails
(Guid Id, string Name, Guid WalletId, string WalletName, Appearance Appearance,
 string TransactionType) : IMapFrom<Category>;