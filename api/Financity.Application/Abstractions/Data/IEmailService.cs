namespace Financity.Application.Abstractions.Data;

public interface IEmailService
{
    public Task SendEmailAsync(string recipientEmailAddress, string subject, string content, CancellationToken ct);
}