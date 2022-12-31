using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Commands;
using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Budgets.Commands;

public sealed class CreateBudgetCommand : ICommand<CreateBudgetCommandResult>, IMapTo<Budget>
{
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }

    public string CurrencyId { get; set; } = string.Empty;

    public HashSet<Guid> TrackedCategoriesId { get; set; } = new();
}

public sealed class CreateBudgetCommandHandler :
    CreateEntityCommandHandler<CreateBudgetCommand, CreateBudgetCommandResult, Budget>
{
    private readonly ICurrentUserService _userService;

    public CreateBudgetCommandHandler(IApplicationDbContext dbContext, IMapper mapper, ICurrentUserService userService)
        : base(dbContext, mapper)
    {
        _userService = userService;
    }

    public override async Task<CreateBudgetCommandResult> Handle(CreateBudgetCommand command,
                                                                 CancellationToken cancellationToken)
    {
        var entity = Mapper.Map<Budget>(command);

        entity.UserId = _userService.UserId;
        entity.TrackedCategories = await DbContext.GetDbSet<Category>()
                                                  .Where(x => command.TrackedCategoriesId.Contains(x.Id))
                                                  .Where(x => x.Wallet.OwnerId == _userService.UserId ||
                                                              x.Wallet.UsersWithSharedAccess.Any(y =>
                                                                  y.Id == _userService.UserId))
                                                  .ToListAsync(cancellationToken);

        DbContext.GetDbSet<Budget>().Add(entity);

        await SaveChanges(cancellationToken);

        return Mapper.Map<CreateBudgetCommandResult>(entity);
    }
}

public sealed record CreateBudgetCommandResult(Guid Id) : IMapFrom<Budget>;