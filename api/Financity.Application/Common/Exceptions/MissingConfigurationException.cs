namespace Financity.Application.Common.Exceptions;

public sealed class MissingConfigurationException : Exception
{
    public MissingConfigurationException(string configurationName) : base(
        $"Missing configuration for {configurationName}")
    {
    }
}