using Financity.Application.Abstractions.Data;
using Financity.Application.Categories.Commands;
using Financity.Application.Common.Extensions;
using Financity.Domain.Entities;
using FluentValidation;

namespace Financity.Application.Categories.Validators;

public sealed class DeleteCategoryValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Id).ExistsForCurrentUser<DeleteCategoryCommand, Category>(dbContext);
    }
}