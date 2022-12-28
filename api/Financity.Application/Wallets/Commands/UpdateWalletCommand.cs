using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Commands;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Wallets.Commands;

public sealed class UpdateWalletCommand : ICommand<Unit>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal StartingAmount { get; set; } = 0;
}

public sealed class UpdateWalletCommandHandler : UpdateEntityCommandHandler<UpdateWalletCommand, Wallet>
{
    public UpdateWalletCommandHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<Unit> Handle(UpdateWalletCommand command, CancellationToken ct)
    {
        var entity = await DbContext.GetDbSet<Wallet>().FirstOrDefaultAsync(x => x.Id == command.Id, ct);

        if (entity is null) throw new EntityNotFoundException(nameof(Wallet), command.Id);

        entity.Name = command.Name;
        entity.StartingAmount = command.StartingAmount;
        
        return await base.Handle(command, ct);
    }
}