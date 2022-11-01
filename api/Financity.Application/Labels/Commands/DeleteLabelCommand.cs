using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Labels.Commands;

public sealed record DeleteLabelCommand(Guid Id) : ICommand<Unit>;

public sealed class DeleteLabelCommandHandler : ICommandHandler<DeleteLabelCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteLabelCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteLabelCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Labels.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null) throw new EntityNotFoundException(nameof(Label), request.Id);

        _dbContext.Labels.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}