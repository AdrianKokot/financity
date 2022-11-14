using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Wallets.Commands;

public sealed class UpdateWalletCommand : ICommand<Unit>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public sealed class UpdateWalletCommandHandler : ICommandHandler<UpdateWalletCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateWalletCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(UpdateWalletCommand request, CancellationToken ct)
    {
        var entity = await _dbContext.GetDbSet<Wallet>().FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (entity is null) throw new EntityNotFoundException(nameof(Wallet), request.Id);

        entity.Name = request.Name;

        await _dbContext.SaveChangesAsync(ct);

        return Unit.Value;
    }
}