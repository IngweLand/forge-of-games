using Microsoft.Extensions.Configuration;

namespace Ingweland.Fog.Functions;

public interface IDynamicConfigurationProvider
{
    void UpdateValue(string key, string value);
    void UpdateValues(Dictionary<string, string> values);
}

public class DynamicConfigurationProvider : ConfigurationProvider, IDynamicConfigurationProvider
{
    public void UpdateValue(string key, string value)
    {
        Data[key] = value;

        OnReload();
    }

    public void UpdateValues(Dictionary<string, string> values)
    {
        foreach (var kvp in values)
        {
            Data[kvp.Key] = kvp.Value;
        }

        OnReload();
    }
}
