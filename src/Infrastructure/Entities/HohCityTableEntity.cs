using System.Runtime.Serialization;
using System.Text.Json;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Shared.Utils;

namespace Ingweland.Fog.Infrastructure.Entities;

public class HohCityTableEntity : TableEntityBase
{
    private HohCity _city;
    private byte[] _cityData;

    [IgnoreDataMember]
    public HohCity City
    {
        get => _city;
        set
        {
            _city = value;
            _cityData = CompressionUtils.CompressString(JsonSerializer.Serialize(value));
        }
    }

    public byte[] CityData
    {
        get => _cityData;
        set
        {
            _cityData = value;
            _city = JsonSerializer.Deserialize<HohCity>(CompressionUtils.DecompressToString(value))!;
        }
    }
}
