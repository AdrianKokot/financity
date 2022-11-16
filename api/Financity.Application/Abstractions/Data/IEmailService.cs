namespace Financity.Application.Abstractions.Data;

public interface IEmailService
{
    public Task SendEmailAsync(string recipientEmailAddress, string subject, string plainTextContent,
                               string? htmlContent, CancellationToken ct);
}