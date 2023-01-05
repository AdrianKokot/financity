using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Helpers;
using Financity.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace Financity.Application.Auth.Commands;

public sealed record ChangePasswordCommand
    (string Password, string NewPassword) : ICommand<ChangePasswordCommandResult>;

public sealed class
    ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, ChangePasswordCommandResult>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<User> _userManager;

    public ChangePasswordCommandHandler(UserManager<User> userManager, ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task<ChangePasswordCommandResult> Handle(ChangePasswordCommand command,
                                                          CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(_currentUserService.NormalizedUserEmail);

        if (user == null || !await _userManager.CheckPasswordAsync(user, command.Password))
            throw ValidationExceptionFactory.For(nameof(command.Password), "User with given credentials doesn't exist");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, command.NewPassword);

        if (!result.Succeeded)
            throw new ValidationException(result.Errors.Select(x => new ValidationFailure(x.Code, x.Description)));

        return new ChangePasswordCommandResult();
    }
}

public sealed record ChangePasswordCommandResult;