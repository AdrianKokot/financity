using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Commands;
using Financity.Application.Common.Mappings;
using Financity.Domain.Entities;

namespace Financity.Application.Labels.Commands;

public sealed class CreateLabelCommand : ICommand<CreateLabelCommandResult>, IMapTo<Label>
{
    public string? Name { get; set; }
    public Guid WalletId { get; set; }
    public string? Color { get; set; }
    public string? IconName { get; set; }
}

public sealed class CreateLabelCommandHandler :
    CreateEntityCommandHandler<CreateLabelCommand, CreateLabelCommandResult, Label>
{
    public CreateLabelCommandHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

public sealed record CreateLabelCommandResult(Guid Id) : IMapFrom<Label>;