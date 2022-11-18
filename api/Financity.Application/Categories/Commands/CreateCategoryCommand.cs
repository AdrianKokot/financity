﻿using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Commands;
using Financity.Domain.Common;
using Financity.Domain.Entities;
using Financity.Domain.Enums;

namespace Financity.Application.Categories.Commands;

public sealed class CreateCategoryCommand : ICommand<CreateCategoryCommandResult>, IMapTo<Category>
{
    public string Name { get; init; } = string.Empty;
    public Appearance Appearance { get; init; } = new();
    public Guid WalletId { get; init; }
    public Guid? ParentCategoryId { get; init; }
    public TransactionType? TransactionType { get; init; }
}

public sealed class CreateCategoryCommandHandler :
    CreateEntityCommandHandler<CreateCategoryCommand, CreateCategoryCommandResult, Category>
{
    public CreateCategoryCommandHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

public sealed record CreateCategoryCommandResult(Guid Id) : IMapFrom<Category>;