using System.Runtime.Serialization;
using System.Text.Json;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Shared.Utils;

namespace Ingweland.Fog.Infrastructure.Entities;

public class InGameStartupDataTableEntity : InGameBinDataTableEntityBase
{
    [IgnoreDataMember]
    public InGameStartupData InGameStartupData
    {
        get =>
            JsonSerializer.Deserialize<InGameStartupData>(
                CompressionUtils.DecompressToString(CombineSegments())) ?? throw new InvalidOperationException();
        init
        {
            ArgumentNullException.ThrowIfNull(value);
            var compressed = CompressionUtils.CompressString(JsonSerializer.Serialize(value));
            SplitIntoSegments(compressed);
        }
    }
}