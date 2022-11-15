using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Entities;
using MediatR;

namespace Financity.Application.Categories.Commands;

public sealed record DeleteCategoryCommand(Guid Id) : ICommand<Unit>;

public sealed class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteCategoryCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteCategoryCommand request,
                                   CancellationToken cancellationToken)
    {
        var deletedCount = await _dbContext.DeleteFromSetAsync<Category>(request.Id, cancellationToken);

        if (deletedCount == 0) throw new EntityNotFoundException(nameof(Category), request.Id);

        return Unit.Value;
    }
}