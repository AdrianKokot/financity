using Microsoft.IdentityModel.Tokens;

namespace Financity.Presentation.Abstractions;

public interface IJwtConfiguration
{
    public bool ValidateIssuer { get; }
    public bool ValidateAudience { get; }
    public bool ValidateLifetime { get; }
    public bool ValidateIssuerSigningKey { get; }
    public string ValidIssuer { get; }
    public string ValidAudience { get; }
    public SymmetricSecurityKey IssuerSigningKey { get; }
    public SigningCredentials Credentials { get; }
    public double ExpireAfterHours { get; }
    public string Algorithm { get; }
}