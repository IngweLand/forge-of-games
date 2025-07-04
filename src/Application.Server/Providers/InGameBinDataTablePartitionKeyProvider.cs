using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Providers;

public class InGameBinDataTablePartitionKeyProvider
{
    public string OtherCity(CityId cityId)
    {
        return $"other-city_{cityId.ToString().ToLowerInvariant()}";
    }
}
