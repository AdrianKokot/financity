using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Commands;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Common;
using Financity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Categories.Commands;

public sealed class UpdateCategoryCommand : ICommand<Unit>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Appearance Appearance { get; set; } = new();
}

public sealed class UpdateCategoryCommandHandler : UpdateEntityCommandHandler<UpdateCategoryCommand, Category>
{
    public UpdateCategoryCommandHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<Unit> Handle(UpdateCategoryCommand command,
                                            CancellationToken cancellationToken)
    {
        var entity = await DbContext.GetDbSet<Category>()
                                    .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (entity is null) throw new EntityNotFoundException(nameof(Category), command.Id);

        entity.Name = command.Name;
        entity.Appearance = command.Appearance;

        return await base.Handle(command, cancellationToken);
    }
}