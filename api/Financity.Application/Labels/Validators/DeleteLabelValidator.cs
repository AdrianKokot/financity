using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Extensions;
using Financity.Application.Labels.Commands;
using Financity.Domain.Entities;
using FluentValidation;

namespace Financity.Application.Labels.Validators;

public sealed class DeleteLabelValidator : AbstractValidator<DeleteLabelCommand>
{
    public DeleteLabelValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Id).HasUserAccess<DeleteLabelCommand, Label>(dbContext);
    }
}