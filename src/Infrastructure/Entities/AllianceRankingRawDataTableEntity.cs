using System.Runtime.Serialization;
using System.Text.Json;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Shared.Utils;

namespace Ingweland.Fog.Infrastructure.Entities;

public class AllianceRankingRawDataTableEntity:TableEntityBase
{
    public required DateTime CollectedAt { get; init; }
    
    private byte[] _data;
    private byte[] _compressedData;

    [IgnoreDataMember]
    public byte[] Data
    {
        get => _data;
        set
        {
            _data = value;
            _compressedData = CompressionUtils.Compress(value);
        }
    }

    public byte[] CompressedData
    {
        get => _compressedData;
        set
        {
            _compressedData = value;
            _data = CompressionUtils.Decompress(value);
        }
    }
}