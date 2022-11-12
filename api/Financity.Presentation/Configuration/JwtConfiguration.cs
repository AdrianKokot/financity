using System.Text;
using Financity.Presentation.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace Financity.Presentation.Configuration;

public sealed class JwtConfiguration : IJwtConfiguration
{
    private string? _key;

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
}