using Microsoft.Extensions.Configuration;

namespace AppCommon
{
    public interface IAppIntegrationConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
