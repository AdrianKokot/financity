using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Financity.Presentation.Auth.Configuration;

public class ConfigureDataProtectionTokenProviderOptions : IConfigureNamedOptions<DataProtectionTokenProviderOptions>
{
    public void Configure(DataProtectionTokenProviderOptions options)
    {
        options.TokenLifespan = TimeSpan.FromHours(2);
    }

    public void Configure(string? name, DataProtectionTokenProviderOptions options)
    {
        Configure(options);
    }
}