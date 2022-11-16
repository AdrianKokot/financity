using Financity.Application.Abstractions.Configuration;
using Financity.Application.Abstractions.Data;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Financity.Infrastructure.Services;

public sealed class EmailService : IEmailService
{
    private readonly SendGridClient _client;
    private readonly ILogger<EmailService> _logger;
    private readonly EmailAddress _sender;

    public EmailService(IEmailConfiguration configuration, ILogger<EmailService> logger)
    {
        _logger = logger;
        _client = new SendGridClient(configuration.Password);
        _sender = new EmailAddress(configuration.From, configuration.Username);
    }

    public async Task SendEmailAsync(string recipientEmailAddress, string subject, string plainTextContent,
                                     string? htmlContent,
                                     CancellationToken ct)
    {
        var to = new EmailAddress(recipientEmailAddress);

        var msg = MailHelper.CreateSingleEmail(_sender, to, subject, plainTextContent, htmlContent ?? plainTextContent);
        var response = await _client.SendEmailAsync(msg, ct);

        if (response.IsSuccessStatusCode)
            _logger.LogError(
                "EmailService: Failed to send email. SendGrid response: {Response}",
                await response.Body.ReadAsStringAsync(ct)
            );
    }
}