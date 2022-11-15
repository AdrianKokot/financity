using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Entities;
using MediatR;

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
        var deletedCount = await _dbContext.DeleteFromSetAsync<Recipient>(request.Id, cancellationToken);

        if (deletedCount == 0) throw new EntityNotFoundException(nameof(Recipient), request.Id);

        return Unit.Value;
    }
}