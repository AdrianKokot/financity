using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Commands;
using Financity.Application.Common.Mappings;
using Financity.Domain.Entities;

namespace Financity.Application.Recipients.Commands;

public sealed class CreateRecipientCommand : ICommand<CreateRecipientCommandResult>, IMapTo<Recipient>
{
    public string? Name { get; init; }
    public Guid WalletId { get; init; }
}

public sealed class CreateRecipientCommandHandler :
    CreateEntityCommandHandler<CreateRecipientCommand, CreateRecipientCommandResult, Recipient>
{
    public CreateRecipientCommandHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

public sealed class CreateRecipientCommandResult : IMapFrom<Recipient>
{
    public Guid Id { get; set; }
}