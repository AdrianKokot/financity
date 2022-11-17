using Financity.Application.Common.Exceptions;

namespace Financity.Presentation.Configuration;

public static class ConfigurationExtensions
{
    public static T BindSingletonConfiguration<TI, T>(this WebApplicationBuilder builder)
        where T : IBindableConfiguration, TI
        where TI : class
    {
        var section =
            typeof(T).GetProperty(nameof(IBindableConfiguration.ConfigurationKey))?.GetValue(null, null)?.ToString()
            ?? throw new NotImplementedException(
                $"Type {typeof(T).Name} doesn't have required property {nameof(IBindableConfiguration.ConfigurationKey)}");

        var config = builder.Configuration.GetSection(section).Get<T>()
                     ?? throw new MissingConfigurationException(section);

        builder.Services.AddSingleton<TI>(config);

        return config;
    }
}