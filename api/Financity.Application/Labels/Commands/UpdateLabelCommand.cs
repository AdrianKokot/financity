using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Common;
using Financity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Labels.Commands;

public sealed class UpdateLabelCommand : ICommand<Unit>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Appearance? Appearance { get; set; }
}

public sealed class UpdateLabelCommandHandler : ICommandHandler<UpdateLabelCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateLabelCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(UpdateLabelCommand request,
                                   CancellationToken cancellationToken)
    {
        var entity = await _dbContext.GetDbSet<Label>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null) throw new EntityNotFoundException(nameof(Label), request.Id);

        entity.Name = request.Name;
        entity.Appearance = request.Appearance ?? new Appearance();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}