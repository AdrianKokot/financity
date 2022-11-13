using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Commands;
using Financity.Application.Common.Mappings;
using Financity.Domain.Common;
using Financity.Domain.Entities;

namespace Financity.Application.Labels.Commands;

public sealed class CreateLabelCommand : ICommand<CreateLabelCommandResult>, IMapTo<Label>
{
    public string? Name { get; set; }
    public Guid WalletId { get; set; }
    public Appearance? Appearance { get; set; }
}

public sealed class CreateLabelCommandHandler :
    CreateEntityCommandHandler<CreateLabelCommand, CreateLabelCommandResult, Label>
{
    public CreateLabelCommandHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public override Task<CreateLabelCommandResult> Handle(CreateLabelCommand command,
                                                          CancellationToken cancellationToken)
    {
        command.Appearance ??= new Appearance();

        return base.Handle(command, cancellationToken);
    }
}

public sealed record CreateLabelCommandResult(Guid Id) : IMapFrom<Label>;