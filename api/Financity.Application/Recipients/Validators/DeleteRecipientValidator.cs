using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Extensions;
using Financity.Application.Recipients.Commands;
using Financity.Domain.Entities;
using FluentValidation;

namespace Financity.Application.Recipients.Validators;

public sealed class DeleteRecipientValidator : AbstractValidator<DeleteRecipientCommand>
{
    public DeleteRecipientValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Id).ExistsForCurrentUser<DeleteRecipientCommand, Recipient>(dbContext);
    }
}