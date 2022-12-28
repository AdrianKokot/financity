using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Commands;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Budgets.Commands;

public sealed class UpdateBudgetCommand : ICommand<Unit>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }

    public HashSet<Guid> TrackedCategoriesId { get; set; } = new();
}

public sealed class UpdateBudgetCommandHandler : UpdateEntityCommandHandler<UpdateBudgetCommand, Budget>
{
    public UpdateBudgetCommandHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<Unit> Handle(UpdateBudgetCommand command,
                                            CancellationToken cancellationToken)
    {
        var entity = await DbContext.GetDbSet<Budget>()
                                    .Include(x => x.TrackedCategories)
                                    .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (entity is null) throw new EntityNotFoundException(nameof(Budget), command.Id);

        entity.Name = command.Name;
        entity.Amount = command.Amount;

        entity.TrackedCategories = await DbContext.GetDbSet<Category>()
                                                  .Where(x => command.TrackedCategoriesId.Contains(x.Id))
                                                  .ToListAsync(cancellationToken);

        return await base.Handle(command, cancellationToken);
    }
}