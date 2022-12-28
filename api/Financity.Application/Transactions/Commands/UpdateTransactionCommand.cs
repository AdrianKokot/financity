using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Commands;
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
    public DateTime TransactionDate { get; set; } = AppDateTime.Now;

    public static void CreateMap(Profile profile)
    {
        profile.CreateMap<CreateTransactionCommand, Transaction>()
               .ForSourceMember(x => x.LabelIds, x => x.DoNotValidate())
               .ForMember(x => x.TransactionDate,
                   x => x.MapFrom(y => DateOnly.FromDateTime(y.TransactionDate.ToUniversalTime()))
               );
    }
}

public sealed class UpdateTransactionCommandHandler : UpdateEntityCommandHandler<UpdateTransactionCommand, Transaction>
{
    public UpdateTransactionCommandHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<Unit> Handle(UpdateTransactionCommand command, CancellationToken ct)
    {
        var entity = await DbContext.GetDbSet<Transaction>()
                                    .Include(x => x.Labels)
                                    .FirstOrDefaultAsync(x => x.Id == command.Id, ct);

        if (entity is null) throw new EntityNotFoundException(nameof(Transaction), command.Id);

        entity.Amount = command.Amount;
        entity.Note = command.Note;

        entity.Recipient = command.RecipientId != null
            ? await DbContext.GetDbSet<Recipient>().FirstAsync(x => x.Id == command.RecipientId, ct)
            : null;
        entity.Category = command.CategoryId != null
            ? await DbContext.GetDbSet<Category>().FirstAsync(x => x.Id == command.CategoryId, ct)
            : null;

        entity.TransactionDate = DateOnly.FromDateTime(command.TransactionDate.ToUniversalTime());

        entity.Labels = await DbContext.GetDbSet<Label>()
                                       .Where(x => command.LabelIds.Contains(x.Id))
                                       .ToListAsync(ct);

        return await base.Handle(command, ct);
    }
}