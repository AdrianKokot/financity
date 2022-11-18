using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Commands;
using Financity.Domain.Common;
using Financity.Domain.Entities;

namespace Financity.Application.Labels.Commands;

public sealed class CreateLabelCommand : ICommand<CreateLabelCommandResult>, IMapTo<Label>
{
    public string Name { get; set; } = string.Empty;
    public Guid WalletId { get; set; }
    public Appearance Appearance { get; set; } = new();
}

public sealed class CreateLabelCommandHandler :
    CreateEntityCommandHandler<CreateLabelCommand, CreateLabelCommandResult, Label>
{
    public CreateLabelCommandHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

public sealed record CreateLabelCommandResult(Guid Id) : IMapFrom<Label>;