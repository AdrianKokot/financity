using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Financity.Presentation.Configuration;

public sealed class JwtConfigurationRegisterNotCalledException : Exception
{
    public JwtConfigurationRegisterNotCalledException() : base(
        $"Configuration key: {JwtConfiguration.ConfigurationKey} is required to bind JwtConfiguration class")

    {
    }
}

public sealed class JwtConfiguration : IJwtConfiguration
{
    private string? _key;
    public static string ConfigurationKey => "Jwt";

    public string? Key
    {
        get => _key;
        set
        {
            _key = value;

            if (string.IsNullOrEmpty(_key)) return;

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            Credentials = new SigningCredentials(IssuerSigningKey, Algorithm);
        }
    }

    public bool ValidateIssuer { get; set; } = true;
    public bool ValidateAudience { get; set; } = true;
    public bool ValidateLifetime { get; set; } = true;
    public bool ValidateIssuerSigningKey { get; set; } = true;
    public string ValidIssuer { get; set; } = string.Empty;
    public string ValidAudience { get; set; } = string.Empty;
    public double ExpireAfterHours { get; set; } = 3;

    public SymmetricSecurityKey? IssuerSigningKey { get; private set; }
    public SigningCredentials? Credentials { get; private set; }
    public string Algorithm => SecurityAlgorithms.HmacSha512;
}