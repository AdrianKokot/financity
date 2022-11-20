using System.Net;
using System.Net.Mail;
using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Configuration;
using Microsoft.Extensions.Options;

namespace Financity.Infrastructure.Services;

public sealed class EmailService : IEmailService
{
    private readonly EmailConfiguration _options;

    public EmailService(IOptions<EmailConfiguration> emailOptions)
    {
        _options = emailOptions.Value;
    }

    public async Task SendEmailAsync(string recipientEmailAddress, string subject, string content,
                                     CancellationToken ct)
    {
        var client = new SmtpClient
        {
            Host = _options.SmtpServer,
            Port = _options.Port,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_options.From, _options.Password)
        };

        var message = new MailMessage(new MailAddress(_options.From, _options.Username),
            new MailAddress(recipientEmailAddress))
        {
            Subject = subject,
            Body = content,
            IsBodyHtml = true
        };

        await client.SendMailAsync(message, ct);
    }
}