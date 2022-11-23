using System.Text;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Helpers;
using Financity.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

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
            throw ValidationExceptionFactory.For("invalidToken", "Invalid token.");

        var result = await _userManager.ResetPasswordAsync(user,
            Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(command.Token)), command.Password);

        if (!result.Succeeded)
            throw new ValidationException(result.Errors.Select(x => new ValidationFailure(x.Code, x.Description)));

        return new ResetPasswordCommandResult();
    }
}

public sealed record ResetPasswordCommandResult;