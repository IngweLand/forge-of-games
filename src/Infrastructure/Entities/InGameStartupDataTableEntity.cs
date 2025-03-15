using System.Runtime.Serialization;
using System.Text.Json;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Shared.Utils;

namespace Ingweland.Fog.Infrastructure.Entities;

public class InGameStartupDataTableEntity : TableEntityBase
{
    private IReadOnlyCollection<HohCity>? _cities;
    private byte[]? _citiesData;
    private BasicCommandCenterProfile? _profile;
    private byte[]? _profileData;

    [IgnoreDataMember]
    public IReadOnlyCollection<HohCity>? Cities
    {
        get => _cities;
        set
        {
            _cities = value;
            _citiesData = value != null
                ? CompressionUtils.CompressString(JsonSerializer.Serialize(value))
                : null;
        }
    }

    public byte[]? CitiesData
    {
        get => _citiesData;
        set
        {
            _citiesData = value;
            _cities = value != null
                ? JsonSerializer.Deserialize<IReadOnlyCollection<HohCity>>(CompressionUtils.DecompressToString(value))
                : null;
        }
    }

    [IgnoreDataMember]
    public BasicCommandCenterProfile? Profile
    {
        get => _profile;
        set
        {
            _profile = value;
            _profileData = value != null
                ? CompressionUtils.CompressString(JsonSerializer.Serialize(value))
                : null;
        }
    }

    public byte[]? ProfileData
    {
        get => _profileData;
        set
        {
            _profileData = value;
            _profile = value != null
                ? JsonSerializer.Deserialize<BasicCommandCenterProfile>(CompressionUtils.DecompressToString(value))
                : null;
        }
    }

    [IgnoreDataMember]
    public string? RelicsJson { get; init; }
}