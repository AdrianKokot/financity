using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Financity.Application.Auth.Commands;

public sealed record RequestResetPasswordCommand(string Email) : ICommand<RequestResetPasswordCommandResult>;

public sealed class
    RequestResetPasswordCommandHandler : ICommandHandler<RequestResetPasswordCommand, RequestResetPasswordCommandResult>
{
    private readonly IEmailService _emailService;
    private readonly UserManager<User> _userManager;

    public RequestResetPasswordCommandHandler(UserManager<User> userManager,
                                              IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    public async Task<RequestResetPasswordCommandResult> Handle(RequestResetPasswordCommand command,
                                                                CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);

        if (user is null)
            return new RequestResetPasswordCommandResult();

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        await _emailService.SendEmailAsync(
            command.Email,
            "Password reset request",
            new ResetPasswordEmailTemplate(token).ToString(),
            ct
        );

        return new RequestResetPasswordCommandResult();
    }
}

public sealed record RequestResetPasswordCommandResult;

internal class ResetPasswordEmailTemplate
{
    private static readonly string FileContent = File.ReadAllText(Path.Combine(Environment.CurrentDirectory,
        "Resources/EmailTemplates/password-reset.template.html"));

    private readonly string _token;

    public ResetPasswordEmailTemplate(string token)
    {
        _token = token;
    }

    public override string ToString()
    {
        return FileContent.Replace("{{expiration-time}}", "2 hours")
                          .Replace("{{action_url}}", $"https://localhost:7025/?token={_token}");
    }
}