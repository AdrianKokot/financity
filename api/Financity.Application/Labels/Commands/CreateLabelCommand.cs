using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Domain.Entities;

namespace Financity.Application.Labels.Commands;

public sealed record CreateLabelCommand
    (string Name, Guid WalletId, string? Color, string? IconName) : ICommand<CreateLabelCommandResult>;

public sealed class CreateLabelCommandHandler : ICommandHandler<CreateLabelCommand, CreateLabelCommandResult>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateLabelCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreateLabelCommandResult> Handle(CreateLabelCommand request,
        CancellationToken cancellationToken)
    {
        Label entity = new()
        {
            Name = request.Name,
            WalletId = request.WalletId
        };

        _dbContext.Labels.Add(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateLabelCommandResult(entity.Id);
    }
}

public sealed record CreateLabelCommandResult(Guid Id);