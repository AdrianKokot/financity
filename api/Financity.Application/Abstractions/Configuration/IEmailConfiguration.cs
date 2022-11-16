namespace Financity.Application.Abstractions.Configuration;

public interface IEmailConfiguration
{
    public string From { get; set; }
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}