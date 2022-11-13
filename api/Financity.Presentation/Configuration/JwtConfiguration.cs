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

    public string Key
    {
        get => _key;
        set
        {
            _key = value;
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(value));
            Credentials = new SigningCredentials(IssuerSigningKey, Algorithm);
        }
    }

    private static JwtConfiguration? Instance { get; set; }

    public bool ValidateIssuer { get; set; }
    public bool ValidateAudience { get; set; }
    public bool ValidateLifetime { get; set; }
    public bool ValidateIssuerSigningKey { get; set; }
    public string? ValidIssuer { get; set; }
    public string? ValidAudience { get; set; }
    public double ExpireAfterHours { get; set; }

    public SymmetricSecurityKey? IssuerSigningKey { get; private set; }
    public SigningCredentials? Credentials { get; private set; }
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