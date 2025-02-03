using System.Text.Json.Serialization;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CityId
{
    Undefined = 0,
    Capital,
    China,
    Egypt,
    Vikings,
}
