using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Extensions;
using Financity.Application.Recipients.Commands;
using Financity.Domain.Entities;
using FluentValidation;

namespace Financity.Application.Recipients.Validators;

public sealed class UpdateRecipientValidator : AbstractValidator<UpdateRecipientCommand>
{
    public UpdateRecipientValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(64);

        RuleFor(x => x.Id).HasUserAccess<UpdateRecipientCommand, Recipient>(dbContext);
    }
}