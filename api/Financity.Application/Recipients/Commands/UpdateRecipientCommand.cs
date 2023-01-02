using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Commands;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Recipients.Commands;

public sealed class UpdateRecipientCommand : ICommand<Unit>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public sealed class UpdateRecipientCommandHandler : UpdateEntityCommandHandler<UpdateRecipientCommand, Recipient>
{
    public UpdateRecipientCommandHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<Unit> Handle(UpdateRecipientCommand request,
                                            CancellationToken cancellationToken)
    {
        var entity = await DbContext.GetDbSet<Recipient>()
                                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null) throw new EntityNotFoundException(nameof(Recipient), request.Id);

        entity.Name = request.Name;

        return await base.Handle(request, cancellationToken);
    }
}