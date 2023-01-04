using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Commands;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Common;
using Financity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Labels.Commands;

public sealed class UpdateLabelCommand : ICommand<Unit>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Appearance Appearance { get; set; } = new();
}

public sealed class UpdateLabelCommandHandler : UpdateEntityCommandHandler<UpdateLabelCommand, Label>
{
    public UpdateLabelCommandHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<Unit> Handle(UpdateLabelCommand command,
                                            CancellationToken cancellationToken)
    {
        var entity = await DbContext.GetDbSet<Label>().FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (entity is null) throw new EntityNotFoundException(nameof(Label), command.Id);

        entity.Name = command.Name;
        entity.Appearance = command.Appearance;

        return await base.Handle(command, cancellationToken);
    }
}