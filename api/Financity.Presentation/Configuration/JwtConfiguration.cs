using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Financity.Presentation.Configuration;

public sealed class JwtConfigurationRegisterNotCalledException : Exception
{
}

public sealed class JwtConfiguration : IJwtConfiguration
{
    private string? _key;

    private JwtConfiguration()
    {
    }

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

    private static JwtConfiguration? Instance { get; set; }

    public bool ValidateIssuer { get; set; } = true;
    public bool ValidateAudience { get; set; } = true;
    public bool ValidateLifetime { get; set; } = true;
    public bool ValidateIssuerSigningKey { get; set; } = true;
    public string ValidIssuer { get; set; } = string.Empty;
    public string ValidAudience { get; set; } = string.Empty;
    public double ExpireAfterHours { get; set; } = 3;

    public SymmetricSecurityKey? IssuerSigningKey { get; private set; } = null;
    public SigningCredentials? Credentials { get; private set; } = null;
    public string Algorithm => SecurityAlgorithms.HmacSha512;

    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        Instance = new JwtConfiguration();
        configuration.Bind("JwtSettings", Instance);
        services.AddSingleton<IJwtConfiguration>(Instance);
    }

    public static JwtConfiguration GetInstance()
    {
        if (Instance is null) throw new JwtConfigurationRegisterNotCalledException();

        return Instance;
    }
}