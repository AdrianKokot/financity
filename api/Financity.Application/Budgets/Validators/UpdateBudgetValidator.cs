﻿using Financity.Application.Abstractions.Data;
using Financity.Application.Budgets.Commands;
using Financity.Application.Common.Extensions;
using Financity.Domain.Entities;
using FluentValidation;

namespace Financity.Application.Budgets.Validators;

public sealed class UpdateBudgetValidator : AbstractValidator<UpdateBudgetCommand>
{
    public UpdateBudgetValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Amount)
            .NotEmpty()
            .Must(x => x >= 0);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.TrackedCategoriesId)
            .ForEach(x => x.NotEmpty())
            .HasUserAccess<UpdateBudgetCommand, Category>(dbContext);

        RuleFor(x => x.Id).UserOwns<UpdateBudgetCommand, Budget>(dbContext);
    }
}