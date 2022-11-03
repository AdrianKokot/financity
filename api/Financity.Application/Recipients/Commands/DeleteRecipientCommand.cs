using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Recipients.Commands;

public sealed record DeleteRecipientCommand(Guid Id) : ICommand<Unit>;

public sealed class DeleteRecipientCommandHandler : ICommandHandler<DeleteRecipientCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteRecipientCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteRecipientCommand request,
                                   CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Recipients.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null) throw new EntityNotFoundException(nameof(Recipient), request.Id);

        _dbContext.Recipients.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}