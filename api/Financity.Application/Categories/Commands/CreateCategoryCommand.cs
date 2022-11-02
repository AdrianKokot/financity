using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Mappings;
using Financity.Domain.Entities;
using Financity.Domain.Enums;

namespace Financity.Application.Categories.Commands;

public sealed class CreateCategoryCommand :
    ICommand<CreateCategoryCommandResult>,
    IMapTo<Category>
{
    public string Name { get; init; }
    public Guid WalletId { get; init; }
    public Guid? ParentCategoryId { get; init; }
    public TransactionType? TransactionType { get; init; }
}

public sealed class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CreateCategoryCommandResult>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<CreateCategoryCommandResult> Handle(CreateCategoryCommand request,
                                                          CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Category>(request);

        _dbContext.Categories.Add(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateCategoryCommandResult(entity.Id);
    }
}

public sealed record CreateCategoryCommandResult(Guid Id);