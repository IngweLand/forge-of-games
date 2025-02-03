using System.Runtime.Serialization;
using System.Text.Json;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Infrastructure.Entities;

public class InGameStartupDataTableEntity : TableEntityBase
{
    private IReadOnlyCollection<HohCity>? _cities;
    private string? _citiesJson;
    private BasicCommandCenterProfile? _profile;
    private string? _profileJson;

    [IgnoreDataMember]
    public BasicCommandCenterProfile? Profile
    {
        get => _profile;
        set
        {
            _profile = value;
            _profileJson = value != null
                ? JsonSerializer.Serialize(value)
                : null;
        }
    }

    public string? ProfileJson
    {
        get => _profileJson;
        set
        {
            _profileJson = value;
            _profile = !string.IsNullOrWhiteSpace(value)
                ? JsonSerializer.Deserialize<BasicCommandCenterProfile>(value)
                : null;
        }
    }

    [IgnoreDataMember]
    public IReadOnlyCollection<HohCity>? Cities
    {
        get => _cities;
        set
        {
            _cities = value;
            _citiesJson = value != null
                ? JsonSerializer.Serialize(value)
                : null;
        }
    }

    public string? CitiesJson
    {
        get => _citiesJson;
        set
        {
            _citiesJson = value;
            _cities = !string.IsNullOrWhiteSpace(value)
                ? JsonSerializer.Deserialize<IReadOnlyCollection<HohCity>>(value)
                : null;
        }
    }

    [IgnoreDataMember]
    public string? RelicsJson { get; init; }
}
