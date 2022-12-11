using System.Collections.Immutable;
using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Commands;
using Financity.Domain.Entities;

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
               .ForSourceMember(x => x.LabelIds, x => x.DoNotValidate());
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
        command.TransactionDate = command.TransactionDate.ToUniversalTime();
        var entity = Mapper.Map<Transaction>(command);

        if (command.ExchangeRate is null) entity.ExchangeRate = 1;

        entity.Labels = DbContext.GetDbSet<Label>()
                                 .Where(x => command.LabelIds.Contains(x.Id))
                                 .ToImmutableArray();

        DbContext.GetDbSet<Transaction>().Add(entity);

        await DbContext.SaveChangesAsync(cancellationToken);

        return Mapper.Map<CreateTransactionCommandResult>(entity);
    }
}

public sealed record CreateTransactionCommandResult(Guid Id) : IMapFrom<Transaction>;