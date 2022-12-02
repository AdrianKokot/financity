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
    public decimal Amount { get; set; } = 0;
    public string Note { get; set; } = string.Empty;
    public Guid? RecipientId { get; set; } = null;
    public Guid? CategoryId { get; set; } = null;
    public HashSet<Guid> LabelIds { get; set; } = new();
    public DateTime? TransactionDate { get; set; } = null;

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

    public async Task<Unit> Handle(UpdateTransactionCommand command, CancellationToken ct)
    {
        var entity = await _dbContext.GetDbSet<Transaction>()
                                     .Include(x => x.Labels)
                                     .FirstOrDefaultAsync(x => x.Id == command.Id, ct);

        if (entity is null) throw new EntityNotFoundException(nameof(Transaction), command.Id);

        entity.Amount = command.Amount;
        entity.Note = command.Note;
        entity.RecipientId = command.RecipientId;
        entity.CategoryId = command.CategoryId;
        if (command.TransactionDate is not null)
            entity.TransactionDate = ((DateTime)command.TransactionDate).ToUniversalTime();

        entity.Labels = await _dbContext.GetDbSet<Label>()
                                        .Where(x => command.LabelIds.Contains(x.Id))
                                        .ToListAsync(ct);

        await _dbContext.SaveChangesAsync(ct);

        return Unit.Value;
    }
}