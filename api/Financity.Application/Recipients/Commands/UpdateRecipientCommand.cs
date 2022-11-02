using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Recipients.Commands;

public sealed class UpdateRecipientCommand : ICommand<Unit>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}

public sealed class UpdateRecipientCommandHandler : ICommandHandler<UpdateRecipientCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateRecipientCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(UpdateRecipientCommand request,
                                   CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Recipients.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null) throw new EntityNotFoundException(nameof(Label), request.Id);

        entity.Name = request.Name;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}