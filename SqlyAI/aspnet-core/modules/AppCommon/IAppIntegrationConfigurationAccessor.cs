using Microsoft.Extensions.Configuration;

namespace App.Integrations
{
    public interface IAppIntegrationConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
