using System.Collections.Immutable;
using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Commands;
using Financity.Application.Common.Mappings;
using Financity.Domain.Entities;
using Financity.Domain.Enums;

namespace Financity.Application.Transactions.Commands;

public sealed class CreateTransactionCommand : ICommand<CreateTransactionCommandResult>, IMapTo<Transaction>
{
    public decimal Amount { get; set; }
    public string? Note { get; set; }

    public Guid? RecipientId { get; set; }
    public Guid WalletId { get; set; }
    public TransactionType TransactionType { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? CurrencyId { get; set; }

    public ICollection<Guid>? LabelIds { get; set; }

    public static void CreateMap(Profile profile)
    {
        profile.CreateMap<CreateTransactionCommand, Transaction>().ForMember(d => d.Labels,
            o =>
            {
                o.MapFrom(c =>
                    ImmutableArray<Label>.Empty);
            });
    }
}

public sealed class CreateTransactionCommandHandler :
    CreateEntityCommandHandler<CreateTransactionCommand, CreateTransactionCommandResult, Transaction>
{
    public CreateTransactionCommandHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

public sealed record CreateTransactionCommandResult(Guid Id) : IMapFrom<Transaction>;