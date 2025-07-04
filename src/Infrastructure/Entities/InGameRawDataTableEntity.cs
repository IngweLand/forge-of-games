using System.Runtime.Serialization;
using Ingweland.Fog.Shared.Utils;

namespace Ingweland.Fog.Infrastructure.Entities;

public class InGameRawDataTableEntity : InGameBinDataTableEntityBase
{
    [IgnoreDataMember]
    public string Base64Data
    {
        get => CompressionUtils.DecompressToString(CombineSegments());
        set
        {
            var compressed = CompressionUtils.CompressString(value);
            SplitIntoSegments(compressed);
        }
    }
}
