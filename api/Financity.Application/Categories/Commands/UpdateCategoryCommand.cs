using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
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

public sealed class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateCategoryCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(UpdateCategoryCommand request,
                                   CancellationToken cancellationToken)
    {
        var entity = await _dbContext.GetDbSet<Category>()
                                     .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null) throw new EntityNotFoundException(nameof(Category), request.Id);

        entity.Name = request.Name;
        entity.Appearance = request.Appearance;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}