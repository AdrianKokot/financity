using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Budgets.Commands;

public sealed class UpdateBudgetCommand : ICommand<Unit>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal Amount { get; set; }

    public HashSet<Guid> TrackedCategoriesId { get; set; } = new();
}

public sealed class UpdateBudgetCommandHandler : ICommandHandler<UpdateBudgetCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateBudgetCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(UpdateBudgetCommand command,
                                   CancellationToken cancellationToken)
    {
        var entity = await _dbContext.GetDbSet<Budget>()
                                     .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (entity is null) throw new EntityNotFoundException(nameof(Budget), command.Id);

        entity.Name = command.Name;
        entity.Amount = command.Amount;

        entity.TrackedCategories = await _dbContext.GetDbSet<Category>()
                                                   .Where(x => command.TrackedCategoriesId.Contains(x.Id))
                                                   .ToListAsync(cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}