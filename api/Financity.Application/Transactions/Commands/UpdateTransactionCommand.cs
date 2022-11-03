using System.Collections.Immutable;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Entities;
using Financity.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Transactions.Commands;

public sealed class UpdateTransactionCommand : ICommand<Unit>
{
    public Guid Id { get; set; }

    public decimal Amount { get; set; }
    public string? Note { get; set; }

    public Guid? RecipientId { get; set; }
    public Guid WalletId { get; set; }
    public TransactionType TransactionType { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? CurrencyId { get; set; }

    public ICollection<Guid>? LabelIds { get; set; }
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

        if (entity is null) throw new EntityNotFoundException(nameof(Label), request.Id);

        entity.Amount = request.Amount;
        entity.Note = request.Note;
        entity.RecipientId = request.RecipientId;
        entity.WalletId = request.WalletId;
        entity.TransactionType = request.TransactionType;
        entity.CategoryId = request.CategoryId;
        entity.CurrencyId = request.CurrencyId;
        entity.Labels = request.LabelIds.Select(id => new Label { Id = id }).ToImmutableArray();

        await _dbContext.SaveChangesAsync(ct);

        return Unit.Value;
    }
}