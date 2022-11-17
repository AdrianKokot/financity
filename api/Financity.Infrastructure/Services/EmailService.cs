using System.Net;
using System.Net.Mail;
using Financity.Application.Abstractions.Configuration;
using Financity.Application.Abstractions.Data;

namespace Financity.Infrastructure.Services;

public sealed class EmailService : IEmailService
{
    private readonly IEmailConfiguration _configuration;

    public EmailService(IEmailConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string recipientEmailAddress, string subject, string content,
                                     CancellationToken ct)
    {
        var client = new SmtpClient
        {
            Host = _configuration.SmtpServer,
            Port = _configuration.Port,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_configuration.From, _configuration.Password)
        };

        var message = new MailMessage(new MailAddress(_configuration.From, _configuration.Username),
            new MailAddress(recipientEmailAddress))
        {
            Subject = subject,
            Body = content,
            IsBodyHtml = true
        };

        await client.SendMailAsync(message, ct);
    }
}