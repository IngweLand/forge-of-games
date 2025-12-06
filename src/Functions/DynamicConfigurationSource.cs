using Microsoft.Extensions.Configuration;

namespace Ingweland.Fog.Functions;

public class DynamicConfigurationSource(DynamicConfigurationProvider provider) : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder) => provider;
}
