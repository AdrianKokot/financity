using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Entities;
using MediatR;

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
        var deletedCount = await _dbContext.DeleteFromSetAsync<Label>(request.Id, cancellationToken);

        if (deletedCount == 0) throw new EntityNotFoundException(nameof(Label), request.Id);

        return Unit.Value;
    }
}