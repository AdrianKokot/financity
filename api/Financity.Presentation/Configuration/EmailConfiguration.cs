using Financity.Application.Abstractions.Configuration;

namespace Financity.Presentation.Configuration;

public sealed class EmailConfiguration : IEmailConfiguration
{
    public static string ConfigurationKey => "Email";
    public string From { get; set; } = string.Empty;
    public string SmtpServer { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}