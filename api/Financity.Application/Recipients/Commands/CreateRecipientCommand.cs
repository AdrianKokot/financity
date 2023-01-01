using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Commands;
using Financity.Domain.Entities;

namespace Financity.Application.Recipients.Commands;

public sealed record CreateRecipientCommand(string Name, Guid WalletId) : ICommand<CreateRecipientCommandResult>,
                                                                           IMapTo<Recipient>;

public sealed class
    CreateRecipientCommandHandler : CreateEntityCommandHandler<CreateRecipientCommand, CreateRecipientCommandResult,
        Recipient>
{
    public CreateRecipientCommandHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

public sealed record CreateRecipientCommandResult(Guid Id) : IMapFrom<Recipient>;