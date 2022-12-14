using System.Collections.Immutable;
using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Commands;
using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Transactions.Commands;

public sealed class CreateTransactionCommand : ICommand<CreateTransactionCommandResult>, IMapTo<Transaction>
{
    public decimal Amount { get; set; } = 0;
    public string Note { get; set; } = string.Empty;
    public float? ExchangeRate { get; set; } = null;
    public Guid? RecipientId { get; set; } = null;
    public Guid WalletId { get; set; } = Guid.Empty;
    public string TransactionType { get; init; } = Domain.Enums.TransactionType.Expense.ToString();
    public Guid? CategoryId { get; set; } = null;
    public string CurrencyId { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; } = AppDateTime.Now;
    public HashSet<Guid> LabelIds { get; set; } = new();

    public static void CreateMap(Profile profile)
    {
        profile.CreateMap<CreateTransactionCommand, Transaction>()
               .ForSourceMember(x => x.LabelIds, x => x.DoNotValidate())
               .ForMember(x => x.TransactionDate,
                   x => x.MapFrom(y => DateOnly.FromDateTime(y.TransactionDate.ToUniversalTime()))
               );
    }
}

public sealed class CreateTransactionCommandHandler :
    CreateEntityCommandHandler<CreateTransactionCommand, CreateTransactionCommandResult, Transaction>
{
    public CreateTransactionCommandHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public override async Task<CreateTransactionCommandResult> Handle(CreateTransactionCommand command,
                                                                      CancellationToken cancellationToken)
    {
        var entity = Mapper.Map<Transaction>(command);

        if (command.ExchangeRate is null) entity.ExchangeRate = 1;

        entity.Labels = DbContext.GetDbSet<Label>()
                                 .Where(x => command.LabelIds.Contains(x.Id))
                                 .ToImmutableArray();

        entity.Currency = await DbContext.GetDbSet<Currency>()
                                         .FirstAsync(x => x.Id == command.CurrencyId, cancellationToken);

        if (command.CategoryId != null)
        {
            entity.Category = await DbContext.GetDbSet<Category>()
                                             .FirstAsync(x => x.Id == command.CategoryId, cancellationToken);
        }

        if (command.RecipientId != null)
        {
            entity.Recipient = await DbContext.GetDbSet<Recipient>()
                                              .FirstAsync(x => x.Id == command.RecipientId, cancellationToken);
        }


        DbContext.GetDbSet<Transaction>().Add(entity);


        await DbContext.SaveChangesAsync(cancellationToken);

        return Mapper.Map<CreateTransactionCommandResult>(entity);
    }
}

public sealed record CreateTransactionCommandResult(Guid Id) : IMapFrom<Transaction>;