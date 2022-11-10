﻿using System.Collections.Immutable;
using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Transactions.Commands;

public sealed class UpdateTransactionCommand : ICommand<Unit>
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
    public Guid? RecipientId { get; set; }
    public Guid CategoryId { get; set; }
    public HashSet<Guid>? LabelIds { get; set; }

    public static void CreateMap(Profile profile)
    {
        profile.CreateMap<CreateTransactionCommand, Transaction>()
               .ForSourceMember(x => x.LabelIds, x => x.DoNotValidate());
    }
}

public sealed class UpdateTransactionCommandHandler : ICommandHandler<UpdateTransactionCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateTransactionCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(UpdateTransactionCommand request, CancellationToken ct)
    {
        var entity = await _dbContext.GetDbSet<Transaction>().FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (entity is null) throw new EntityNotFoundException(nameof(Transaction), request.Id);

        entity.Amount = request.Amount;
        entity.Note = request.Note;
        entity.RecipientId = request.RecipientId;
        entity.CategoryId = request.CategoryId;

        entity.Labels = _dbContext.GetDbSet<Label>()
                                  .Where(x => request.LabelIds.Contains(x.Id))
                                  .ToImmutableArray();

        await _dbContext.SaveChangesAsync(ct);

        return Unit.Value;
    }
}