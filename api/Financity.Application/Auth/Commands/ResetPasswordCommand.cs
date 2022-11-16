﻿using Financity.Application.Abstractions.Messaging;
using Financity.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace Financity.Application.Auth.Commands;

public sealed record ResetPasswordCommand
    (string Password, string Token, string Email) : ICommand<ResetPasswordCommandResult>;

public sealed class
    ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, ResetPasswordCommandResult>
{
    private readonly UserManager<User> _userManager;

    public ResetPasswordCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ResetPasswordCommandResult> Handle(ResetPasswordCommand command,
                                                         CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);

        if (user is null)
            return new ResetPasswordCommandResult();

        var result = await _userManager.ResetPasswordAsync(user, command.Token, command.Password);

        if (!result.Succeeded)
            throw new ValidationException(result.Errors.Select(x => new ValidationFailure(x.Code, x.Description)));

        return new ResetPasswordCommandResult();
    }
}

public sealed record ResetPasswordCommandResult;