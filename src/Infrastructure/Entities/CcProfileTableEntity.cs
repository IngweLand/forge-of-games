using System.Runtime.Serialization;
using System.Text.Json;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Infrastructure.Entities;

public class CcProfileTableEntity : TableEntityBase
{
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
}
