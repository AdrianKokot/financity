using System.Text;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Configuration;
using Financity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace Financity.Application.Auth.Commands;

public sealed record RequestResetPasswordCommand(string Email) : ICommand<RequestResetPasswordCommandResult>;

public sealed class
    RequestResetPasswordCommandHandler : ICommandHandler<RequestResetPasswordCommand, RequestResetPasswordCommandResult>
{
    private readonly IEmailService _emailService;
    private readonly IOptions<EmailConfiguration> _emailOptions;
    private readonly UserManager<User> _userManager;

    public RequestResetPasswordCommandHandler(UserManager<User> userManager,
                                              IEmailService emailService, IOptions<EmailConfiguration> emailOptions)
    {
        _userManager = userManager;
        _emailService = emailService;
        _emailOptions = emailOptions;
    }

    public async Task<RequestResetPasswordCommandResult> Handle(RequestResetPasswordCommand command,
                                                                CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);

        if (user is null)
            return new RequestResetPasswordCommandResult();

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        await _emailService.SendEmailAsync(
            command.Email,
            "Password reset request",
            new ResetPasswordEmailTemplate(token, _emailOptions.Value.AppUrl).ToString(),
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
    private readonly string _url;

    public ResetPasswordEmailTemplate(string token, string url)
    {
        _token = token;
        _url = url;
    }

    public override string ToString()
    {
        return FileContent.Replace("{{expiration-time}}", "2 hours")
                          .Replace("{{action_url}}", $"{_url}/auth/reset-password?token={_token}");
    }
}